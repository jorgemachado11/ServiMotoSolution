using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace MotorcycleClient
{
    public class ContextMenu
    {
        public static async System.Threading.Tasks.Task Show(ContextMenuOptions options)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet(
                options.Title,
                options.Cancel,
                options.Destruction,
                options.Actions);

            options.OnActionSelected?.Invoke(action);
        }
    }

    public class ContextMenuOptions
    {
        public string Title { get; set; }
        public string Cancel { get; set; }
        public string Destruction { get; set; }
        public string[] Actions { get; set; }
        public Action<string> OnActionSelected { get; set; }
    }
}
