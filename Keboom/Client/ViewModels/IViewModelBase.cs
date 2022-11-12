using System.ComponentModel;

namespace Keboom.Client.ViewModels;

public interface IViewModelBase : INotifyPropertyChanged
{
    Task OnInitializedAsync();
    Task Loaded();
}