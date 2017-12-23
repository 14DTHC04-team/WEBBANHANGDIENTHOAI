using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanHangDienThoai.Startup))]
namespace WebBanHangDienThoai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
