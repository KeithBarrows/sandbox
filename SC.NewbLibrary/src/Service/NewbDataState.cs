using SC.NewbLibrary.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC.NewbLibrary.Service
{
    public class NewbDataState
    {
        private readonly List<NewbViewData> _data = new List<NewbViewData>();
        public IReadOnlyList<NewbViewData> Data => _data;

        public event Action OnDataAdded;
        public event Action OnDataUpdated;

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

        private void StateChangedAdd() => OnDataAdded?.Invoke();
        private void StateChangedUpdate() => OnDataUpdated?.Invoke();
    }
}
