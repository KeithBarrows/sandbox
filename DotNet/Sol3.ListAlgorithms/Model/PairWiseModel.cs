using System.Collections.Generic;
using System.Linq;

namespace Sol3.ListAlgorithms.Model
{
    public class PairwiseModel
    {
        private List<string> _items;
        private List<PairModel> _pairs;

        public PairwiseModel()
        {
            _items = new List<string>();
            _pairs = new List<PairModel>();
        }

        public void AddItem(string item) => _items.Add(item);
        public void PrepForSort()
        {
            for (int vert = 0; vert < _items.Count; vert++)
            {
                for (int horz = 0; horz < _items.Count; horz++)
                {
                    if (horz > vert)
                        _pairs.Add(new PairModel { Title1 = _items[horz], Title2 = _items[vert] });
                }
            }
        }
        public List<PairModel> GetListToCompare() => _pairs;
        public Dictionary<string, int> DoSort(List<PairModel> pairs)
        {
            var result = new Dictionary<string, int>();
            foreach (var item in _items)
            {
                var ab = pairs.Where(a => a.Title1 == item && a.AB > 0).Sum(a => a.AB);
                var ba = pairs.Where(a => a.Title1 == item && a.BA > 0).Sum(a => a.BA);
                result.Add(item, ab + ba);
            }
            return result;
        }

        public class PairModel
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public int AB { get; set; } = -1;
            public int BA => AB < 0 ? -1 : (AB > 0 ? 0 : 1);
        }
    }
}
