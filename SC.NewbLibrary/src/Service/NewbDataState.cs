using SC.NewbLibrary.Model.Data;
using SC.NewbLibrary.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC.NewbLibrary.Service
{
    public class NewbDataState
    {
        #region Properties
        private /*readonly*/ List<NewbViewData> _data;
        private List<NewbViewData> _filteredData;
        public IReadOnlyList<NewbViewData> NewbData => _data.OrderBy(a => a.Term).ToList();
        public IReadOnlyList<NewbViewData> FilteredData => _filteredData.OrderBy(a => a.Term).ToList();
        public string SearchLetter { get; private set; }
        public NewbViewData EmptyDataPoint { get; private set; } = new NewbViewData();
        #endregion

        #region Event declarations
        public event Action OnStateChanged;
        private void StateChanged() => OnStateChanged?.Invoke();
        #endregion

        #region ctor
        public NewbDataState()
        {
            var service = new NewbDataService();
            _data = service.GetNewbData().Result;
            _filteredData = _data;
        }
        #endregion

        #region Add data handling
        public bool AddMode { get; private set; } = false;
        public void ToggleAddMode()
        {
            AddMode = !AddMode;
            StateChanged();
        }
        public void DismissAddMode()
        {
            AddMode = false;
            StateChanged();
        }
        public void SaveData(NewbDataEvent newEventData, NewbViewData dataPoint = null)
        {

            if (!string.IsNullOrWhiteSpace(newEventData.EventDefinition) ||
                !string.IsNullOrWhiteSpace(newEventData.EventLink) ||
                !string.IsNullOrWhiteSpace(newEventData.EventTerm) ||
                !string.IsNullOrWhiteSpace(newEventData.EventTerse))
            {
                newEventData.EventStamp = DateTime.UtcNow;
                newEventData.EventUser = "Anonymous";       // TODO:  Need the EVE ESI Login Integration piece!

                if (dataPoint != null || _data.Any(a => a.Term.Equals(newEventData.EventTerm, StringComparison.OrdinalIgnoreCase)))  // this is an edit
                {
                    if (dataPoint == null)
                        dataPoint = _data.FirstOrDefault(a => a.Term.Equals(newEventData.EventTerm, StringComparison.OrdinalIgnoreCase));
                    dataPoint.EventHistory.Add(newEventData);
                    dataPoint.DismissEditMode();
                }
                else
                {
                    dataPoint = new NewbViewData { Term = newEventData.EventTerm, };
                    dataPoint.EventHistory.Add(newEventData);
                    _data.Add(dataPoint);
                }

                AddMode = false;
                SetFilter(SearchLetter);
                StateChanged();
            }
        }
        #endregion

        #region Update data handling
        public void RaiseStateChangedEvent()
        {
            StateChanged();
        }
        public void UpdateData()
        {

        }
        #endregion

        public void SetFilter(string filter)
        {
            SearchLetter = filter;
            if (SearchLetter.Length == 1 && (int)SearchLetter.ToUpper()[0] >= (int)'A' && (int)SearchLetter.ToUpper()[0] <= (int)'Z')
                _filteredData = _data.Where(a => a.Term.StartsWith(SearchLetter, StringComparison.OrdinalIgnoreCase)).ToList();
            else
                _filteredData = _data;
            StateChanged();
        }
    }
}
