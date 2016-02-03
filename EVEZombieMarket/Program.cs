using System;
using System.Linq;
using eZet.EveLib.EveCrestModule;

namespace EVEZombieMarket
{
    /// <summary>
    /// A tiny demo that, for the region Deklein, prints out the volume for the
    /// antimatter charge S item.
    /// 
    /// See https://wiki.eveonline.com/en/wiki/CREST_Getting_Started for a
    /// description of what is available here. The general URLs map to items
    /// in the API in a more or less straightfoward way, e.g., market/types
    /// seems to map to root.MarketTypes. Parameterized URLs (e.g., with specific
    /// ids, etc.) I have yet to figure out beyond the end to end helper functions.
    /// </summary>

    public static class Program
    {
        public static void Main(string[] args)
        {
            // There are alternate views for things requiring a preamble.
            var crest = new EveCrest();
            var root = crest.GetRoot();

            // Note collections hold only the first 1000 or so items. To get an enumerable
            // over all, one might use the AllItems utility method.

            // The following is an illustration of one way to get the region ID for a given
            // named region.
            var regions = root.Query(r => r.Regions);
            const string regionName = "Deklein";
            var regionId = regions.AllItems().First(r => r.Name == regionName).Id;
            Console.WriteLine("Id for region '{0}' is {1}", regionName, regionId);

            // Now get the market type ID for generic small antimatter.
            var marketTypes = crest.Load(root.MarketTypes);
            const string itemName = "Antimatter Charge S";
            var itemId = marketTypes.AllItems().Select(i => i.Type).First(t => t.Name == itemName).Id;
            Console.WriteLine("Id for market item '{0}' is {1}", itemName, itemId);

            // The following call is deprecated as it is not idiomatic w.r.t. CREST, but it's
            // unclear to me what its replacement actually is.
            var hist = crest.GetMarketHistory(regionId, itemId);
            double monies = 0;
            long items = 0;
            foreach (var item in hist.AllItems())
            {
                monies += item.AvgPrice * item.Volume;
                items += item.Volume;
            }
            // These reports seem to span 13 total months.
            Console.WriteLine("Avg for '{0}' in '{1}' is {2} across {3} total ISK volume", itemName, regionName, monies / items, monies);

            // That's actually rather more than I expected. Who finds it effective to brawl with
            // antimatter charge S in nullsec, against the NPCs we often see here?
        }
    }
}
