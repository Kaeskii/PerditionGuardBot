using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using PerditionGuardBot.Commands;
using PerditionGuardBot.Commands.Basic;
using PerditionGuardBot.Commands.ProfileSystem;
using PerditionGuardBot.config;
using PerditionGuardBot.TicketsSystem;

namespace PerditionGuardBot
{
    internal class ProgramMain
    {
        // Notes:
        // I want all the permission related Commands to send me a seperate DM with the user's name and the command they used. This makes an easy way to track who is using the bot and what they are doing with it.
        // It also prevents missuse of the bot.
        //
        //
        //
        //
        //
        //
        public static DiscordClient? Client { get; set; }

        private static CommandsNextExtension Commands;

        // this if for the Commands properties
        public static CommandsNextExtension GetCommands()
        {
            return Commands;
        }

        // this if for the Commands properties
        public static void SetCommands(CommandsNextExtension value)
        {
            Commands = value;
        }

        public static LoggingSystem? logger;
        private static Dictionary<string, ulong> Vcids = new();
        // gonna need one for profiles (dictionary)
        static async Task Main(string[] args)
        {
            logger = LoggingSystem.Instance;

            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1) // this is the default timout for all of the interactions except ones with overrides for the specific command
            });

            // Set all EVENTS to be tracked here (event handle area)
            Client.Ready += Client_Ready;
            Client.VoiceStateUpdated += VoiceChannelHandling;
            Client.MessageCreated += MessageCreationHandling;
            Client.GuildMemberAdded += WelcomeMessage;
            Client.GuildMemberRemoved += LeaveMessage;
            // This is for interactable buttons do not touch.
            Client.ComponentInteractionCreated += ComponentCreationHandling;
            Client.ModalSubmitted += ModalCreationHandling;

            // (end of event handle area)
            // The bot sets up properly here

            var CommandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
                QuotationMarks = "" // if anything breaks it's this (command wise)
            };

            SetCommands(Client.UseCommandsNext(CommandsConfig));
            // for all the test Commands
            GetCommands().RegisterCommands<TestCommands>();
            // registering other command areas
            GetCommands().RegisterCommands<Administration>();
            GetCommands().RegisterCommands<Moderation>();
            GetCommands().RegisterCommands<TicketCommands>();
            // prefix Commands and server settings
            GetCommands().RegisterCommands<Configuration>();
            GetCommands().RegisterCommands<Everyone>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        // use arg here instead of ctx because these ARE NOT Commands they are EVENTS
        private static async Task VoiceChannelHandling(DiscordClient send, VoiceStateUpdateEventArgs? arg) // vc events
        {
            await Task.CompletedTask;
            // these are bugging out right now
        }

        private static async Task MessageCreationHandling(DiscordClient client, MessageCreateEventArgs arg) // the blacklist is a class to keep ProgramMain clean.
        {
            if (!arg.Author.IsBot)
            {
                // blacklist
                var blacklist = new Blacklist();
                foreach (var blword in blacklist.blacklist)
                {
                    if (arg.Message.Content.ToLower().Contains(blword)) // wildcard (works anywhere in a messge) and not case sensitive.
                    {
                        await arg.Message.DeleteAsync();
                        var message = await arg.Channel.SendMessageAsync("don't say that here.");
                        await Task.Delay(10000); // waits 10 seconds
                        await arg.Channel.DeleteMessageAsync(message); // deletes
                    }
                }
                // ticket message logger
                var messages = new Transcripts
                {
                    Username = arg.Author.Username,
                    ChannelId = arg.Channel.Id,
                    Content = arg.Message.Content,
                    SendDate = DateTime.Now,
                };
                logger.messages.Add(messages);
                // xp and level system
                var ProfileConstruct = new ProfileConstruct();
                ProfileConstruct.AddXP(arg.Author.Username, arg.Author.Id, 1); // last value is the xp per message
                if (ProfileConstruct.levelup)
                {
                    var levelupmessage = await arg.Channel.SendMessageAsync($"{arg.Author.Mention} has leveled up to level {ProfileConstruct.GetUserInfo(arg.Author.Username, arg.Author.Id).Level}!");
                    await Task.Delay(5000);
                    await arg.Channel.DeleteMessageAsync(levelupmessage);
                }
            } // I can probably use this for an image only channel too.
        }

        private static async Task WelcomeMessage(DiscordClient client, GuildMemberAddEventArgs arg) // join server message (needs some improvement) (not working as of march, I suspect profile creation is the porblem as it worked before that implimentation)
        {
            var UserInfo = new StoredUserInfo()
            {
                UserName = arg.Member.Username,
                UserID = arg.Member.Id,
                AvatarURL = arg.Member.AvatarUrl,
                Level = 1,
                XP = 0,
                Bans = 0,
                Kicks = 0,
                Warnings = 0,
                McbeCheater = false,
                McbeCheaterCaughtBy = "Null",
                IsBooster = false,
            };
            var ProfileConstruct = new ProfileConstruct();
            var DoesExist = ProfileConstruct.CheckIfUserAlreadyExists(arg.Member.Username, arg.Member.Id);
            string ProfileMessage = "";
            if (DoesExist == false)
            {
                var Stored = ProfileConstruct.StoreUserInfo(UserInfo);
                if (Stored == true)
                {
                    ProfileMessage = "pc";
                }
                else
                {
                    ProfileMessage = "epc";
                }
            }
            if (DoesExist == true)
            {
                ProfileMessage = "pae";
            }
            var jchannel = arg.Guild.GetDefaultChannel();
            if (arg.Guild.Id == 964579124985352192) // perdition discord server
            {
                jchannel = arg.Guild.GetChannel(965331297591525396);
            }
            var welcome = new DiscordEmbedBuilder()
            {
                Color = Settings.GetPrimaryColor(),
                Title = $"Welcome {arg.Member.Username} to {arg.Guild.Name}.",
                Description = $"Please ensure you follow typical common sense if you have any.",
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = ProfileMessage + " | Enjoy your stay" },
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = arg.Member.AvatarUrl }
            };
            await jchannel.SendMessageAsync(embed: welcome);
        }

        private static async Task LeaveMessage(DiscordClient client, GuildMemberRemoveEventArgs arg) // leave server message
        {
            var dchannel = arg.Guild.GetDefaultChannel();
            if (arg.Guild.Id == 964579124985352192)
            {
                dchannel = arg.Guild.GetChannel(965331297591525396);
            }
            var left = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Black,
                Title = $"{arg.Member.Username} escaped {arg.Guild.Name}.",
                Description = $"They have moved onto a better place.",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = arg.Member.AvatarUrl }
            };
            await dchannel.SendMessageAsync(embed: left);
        }

        private static async Task ComponentCreationHandling(DiscordClient client, ComponentInteractionCreateEventArgs arg)
        {
            var TicketConstruct = new TicketConstruct();
            switch (arg.Interaction.Data.CustomId)
            {
                // begining of ticket buttons
                case "helpButton":
                    var openModal = new DiscordInteractionResponseBuilder()
                        .WithCustomId("openModalForm")
                        .WithTitle("Open support ticket")
                        .AddComponents(new TextInputComponent("Include all relevant details: ", "openTextBox"));
                    await arg.Interaction.CreateResponseAsync(InteractionResponseType.Modal, openModal);
                    break;
                case "viewButton":
                    var tickets = TicketConstruct.GetTickets();
                    int count = 0;
                    string[] tempList = new string[tickets.Count];
                    foreach (var ticket in tickets)
                    {
                        tempList[count] = $"**Ticket-**{ticket.TicketNo}, **TicketID:** {ticket.TicketId}, **Opened By:** {ticket.User}";
                        count++;
                    }
                    var ticketsView = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = "Normal Tickets:",
                        Description = string.Join("\n", tempList)
                    };
                    await arg.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(ticketsView));
                    break;
                case "deleteIdButton":
                    var deleteId = new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.White)
                        .WithTitle("Enter ticket ID");
                    await arg.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(deleteId));
                    var ID = await arg.Channel.GetNextMessageAsync();
                    var isDeleted = TicketConstruct.DeleteTicket(ulong.Parse(ID.Result.Content));
                    if (isDeleted == true)
                    {
                        var success = new DiscordEmbedBuilder()
                        {
                            Color = Settings.GetPrimaryColor(),
                            Title = $"Deleted ticket: {ID.Result.Content}"
                        };
                        await arg.Channel.SendMessageAsync(embed: success);
                    }
                    else
                    {
                        var failed = new DiscordEmbedBuilder()
                        {
                            Color = DiscordColor.Red,
                            Title = $"**{ID.Result.Content}**, is not a valid ticket."
                        };
                        await arg.Channel.SendMessageAsync(embed: failed);
                    }
                    break;
            }
        }

        private static async Task ModalCreationHandling(DiscordClient client, ModalSubmitEventArgs arg)
        {
            var modalValues = arg.Values;
            if (arg.Interaction.Type == InteractionType.ModalSubmit && arg.Interaction.Data.CustomId == "openModalForm") // this creates the normal tickets
            {
                var random = new Random();
                var ticketConstruct = new TicketConstruct();
                ulong minV = 1000000000000000000;
                ulong maxV = 9999999999999999999;
                ulong randomId = (ulong)random.Next((int)(minV >> 32), int.MaxValue) << 32 | (ulong)random.Next();
                ulong result = randomId % (maxV - minV + 1) + minV;
                var openTicket = new NormalTickets()
                {
                    User = arg.Interaction.User.Username,
                    Content = modalValues.Values.First(),
                    TicketNo = ticketConstruct.TotTickets() + 1,
                    TicketId = result
                };
                var openChannel = await arg.Interaction.Guild.CreateChannelAsync($"Ticket-{openTicket.TicketNo}", ChannelType.Text, arg.Interaction.Channel.Parent); // ticket creation
                ticketConstruct.Transcript(openTicket);
                var empty = new DiscordEmbedBuilder()
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = $"{arg.Interaction.User.Username} opened a ticket"
                };
                var contextEmbed = new DiscordEmbedBuilder() // will change this to have it's own control pannel will need an overhaul for the embed building though
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = $"{arg.Interaction.User.Username} opened a ticket.",
                    Description = $"**Context:** {modalValues.Values.First()} \n\n**Ticket ID:** {openTicket.TicketId}"
                };
                await openChannel.SendMessageAsync(embed: contextEmbed);
                await arg.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(empty));
                await Task.Delay(3000);
                // method needed to delete the embed here but I don't know how
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) // runs after everything had loaded which actually starts the bot usually takes less than a second
        {
            return Task.CompletedTask; // reports when tasks have been completed in the console (Commands)
        }
    }
}