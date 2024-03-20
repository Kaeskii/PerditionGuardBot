using Newtonsoft.Json;

namespace PerditionGuardBot.config
{
    internal class JSONReader // you shouldn't need to ever edit this code (unless the json just doesn't work for some reason)
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public async Task ReadJSON()
        {
            using (StreamReader sr = new("config.json"))
            {
                // reads the json
                string json = await sr.ReadToEndAsync();
                JSONStrcture? data = JsonConvert.DeserializeObject<JSONStrcture>(json); // this literally won't be null why is the console crying at me
                // extracts the data
                this.Token = data.Token;
                this.Prefix = data.Prefix;
            }
        }
    }
    internal sealed class JSONStrcture
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
    }
}