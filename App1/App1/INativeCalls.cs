using System.Threading.Tasks;

namespace App1
{
    public interface INativeCalls
    {
        Task OpenToast(string title, string text, string accept = "OK");
        Task<string> OpenInputToast(string title, string text, string placeholder, string accept = "OK", string cancel = "Cancel");
    }
}
