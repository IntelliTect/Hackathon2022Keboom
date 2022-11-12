using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Keboom.Client.ViewModels;

public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    public virtual async Task OnInitializedAsync()
    {
        await Loaded().ConfigureAwait(true);
    }


    [RelayCommand]
    public virtual async Task Loaded()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }
}