using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    internal class UserAgreementViewModel : ViewModelBase
    {
        public UserAgreementViewModel(INavigationService navigationService)
        {
            CloseCommand = new Command(async () => await navigationService.PopModalAsync());

            Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque a tortor quis purus maximus lobortis vel ut risus. Ut lacinia rutrum fringilla. Donec aliquet, nulla sodales posuere pretium, mi odio facilisis tortor, et ullamcorper erat ante in sem. Aenean accumsan, diam a eleifend accumsan, ante massa ultrices justo, vitae feugiat orci libero ut nisl. Integer vel nisl urna. Sed ornare metus ac urna consectetur lobortis. Mauris interdum est ac dolor fermentum, sit amet fermentum dolor sollicitudin. Quisque lacinia massa lectus, sed pharetra orci facilisis vel. Maecenas odio quam, rutrum vitae risus quis, semper elementum ipsum. Nulla gravida quam vitae urna tempor consequat. Sed a nisi mauris. Sed augue magna, suscipit placerat ultricies hendrerit, faucibus a eros. Nullam tristique venenatis mi, at efficitur diam placerat at. Aenean efficitur elit scelerisque purus interdum euismod. Cras consequat neque et lectus tincidunt ultricies. Etiam vel quam vitae mauris tempor dapibus.

Quisque sollicitudin dapibus nulla. Nulla tempor condimentum lacus, vel vehicula neque. Suspendisse potenti. Phasellus feugiat, nulla eget viverra mattis, ex ante mollis velit, in malesuada ex elit id velit. Sed eget pellentesque turpis. Pellentesque sed vehicula ligula, eu rutrum massa. Aenean ut tellus id ligula finibus vulputate sit amet ut tellus. Cras congue vitae urna at laoreet.

Donec eu dolor sapien. Nulla iaculis risus ante, sit amet pellentesque risus imperdiet eu. Cras pellentesque ipsum quam, vitae pretium urna euismod vel. Duis dictum, metus in dignissim molestie, lacus ipsum scelerisque leo, ac consequat mauris lacus eu velit. Fusce egestas, elit at ornare porta, velit purus cursus justo, eget sollicitudin nunc mi nec risus. Pellentesque hendrerit luctus purus, in luctus nunc lacinia sit amet. Nulla facilisi. Aenean et augue rutrum, lacinia nisi vitae, faucibus arcu. Interdum et malesuada fames ac ante ipsum primis in faucibus. Suspendisse potenti. Suspendisse nibh nunc, lacinia non tincidunt vel, aliquet quis elit. Ut faucibus mattis hendrerit.

Duis nec ligula molestie, porttitor ex ac, eleifend felis. Suspendisse nunc turpis, eleifend non dui non, accumsan scelerisque odio. Phasellus porttitor finibus congue. Nullam nec luctus leo, a tristique nisi. Sed dui libero, rutrum quis lobortis nec, pellentesque eget quam. Donec et mollis est. Nam blandit, nulla et gravida posuere, risus sapien aliquam urna, nec luctus dui felis at magna. Suspendisse lobortis dolor vel lectus feugiat, eu rutrum quam interdum. Ut facilisis tempus tincidunt. Donec porttitor vitae risus at rhoncus. Morbi finibus lorem nec aliquet facilisis. Donec aliquet felis venenatis, consectetur diam sit amet, porta dolor. Maecenas nec diam consequat, feugiat neque eleifend, pulvinar odio. Morbi volutpat massa sodales arcu aliquam ullamcorper. Donec at euismod augue.

Pellentesque gravida libero enim, sit amet vestibulum metus eleifend fringilla. Vivamus et quam odio. Mauris purus est, vestibulum at augue et, blandit cursus lorem. Proin condimentum, neque id efficitur tempor, est augue ornare massa, non consequat elit quam ultrices justo. Nam nec mi consectetur nisi faucibus bibendum. Nam purus ipsum, fringilla vel est sed, finibus scelerisque diam. In nec felis volutpat, venenatis ligula id, accumsan felis. Aenean tincidunt mi id arcu ullamcorper volutpat. Suspendisse convallis augue a placerat cursus. Nulla quis ipsum vitae felis ullamcorper dictum. Ut vel justo ultrices, vestibulum urna nec, commodo sem. Cras vitae dignissim eros.";

        }

        public string Text { get; }

        public Command CloseCommand { get; }
    }
}
