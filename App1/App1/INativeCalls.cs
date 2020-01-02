using System.Threading.Tasks;

namespace App1
{
    public interface INativeCalls
    {
        void OpenToast(string title, string text, string accept = "OK");
        Task<string> OpenInputToast(string title, string text, string inputPlaceholder, string accept = "OK", string cancel = "Cancel");
    }
}
