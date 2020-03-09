using SC.NewbLibrary.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC.NewbLibrary.Service
{
    public class NewbDataState
    {
        private /*readonly*/ List<NewbViewData> _data;
        private List<NewbViewData> _filteredData;
        public IReadOnlyList<NewbViewData> NewbData => _data;
        public IReadOnlyList<NewbViewData> FilteredData => _filteredData;
        public string SearchLetter { get; private set; }
        public NewbViewData EmptyDataPoint { get; private set; } = new NewbViewData();

        public event Action OnFilterRequested;
        public event Action OnDataAdded;
        public event Action OnDataUpdated;


        public NewbDataState()
        {
            var service = new NewbDataService();
            _data = service.GetNewbData().Result;
            _filteredData = _data;
        }

        public void SetFilter(string filter)
        {
            SearchLetter = filter;
            if (SearchLetter.Length == 1 && (int)SearchLetter.ToUpper()[0] >= (int)'A' && (int)SearchLetter.ToUpper()[0] <= (int)'Z')
                _filteredData = _data.Where(a => a.Term.StartsWith(SearchLetter, StringComparison.OrdinalIgnoreCase)).ToList();
            else
                _filteredData = _data;
            StateChangedFiltered();
        }


        public void FilterData()
        {
            StateChangedFiltered();
        }
        public void AddData(NewbViewData data)
        {
            _data.Add(data);
            StateChangedAdd();
        }
        public void UpdateData(NewbViewData data)
        {
            var dataToBeUpdated = _data.FirstOrDefault(a => a.Term == data.Term);
            if (dataToBeUpdated != null)
            {
                dataToBeUpdated = data;
                StateChangedUpdate();
            }
        }

        private void StateChangedFiltered() => OnFilterRequested?.Invoke();
        private void StateChangedAdd() => OnDataAdded?.Invoke();
        private void StateChangedUpdate() => OnDataUpdated?.Invoke();
    }
}
