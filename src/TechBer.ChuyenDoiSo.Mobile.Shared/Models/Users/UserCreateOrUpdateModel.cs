using System.ComponentModel;
using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Authorization.Users.Dto;

namespace TechBer.ChuyenDoiSo.Models.Users
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}