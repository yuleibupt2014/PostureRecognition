using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Chronos_As_Read
{
    class Music
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")]
        static extern byte MapVirtualKey(byte wCode, int wMap);
        public void Music_Open()      //ctrl+shif+F10打开音乐盒
        {
            keybd_event(17, MapVirtualKey(17, 0), 0, 0); //按下CTRL           
            keybd_event(16, MapVirtualKey(16, 0), 0, 0);//键下shift键。
            keybd_event(121, MapVirtualKey(120, 0), 0, 0);//键下F10键。
            keybd_event(17, MapVirtualKey(17, 0), 0x2, 0); //放开CTRL
            keybd_event(16, MapVirtualKey(16, 0), 0x2, 0);//放开shift键。
            keybd_event(121, MapVirtualKey(120, 0), 0x2, 0);//放开F10键。
        }
        public void Music_Pauseorbroadcast()         //ctrl+shif+F5暂停或播放
        {
            keybd_event(17, MapVirtualKey(17, 0), 0, 0); //按下CTRL           
            keybd_event(16, MapVirtualKey(16, 0), 0, 0);//键下shift键。
            keybd_event(116, MapVirtualKey(116, 0), 0, 0);//键下F5键。
            keybd_event(17, MapVirtualKey(17, 0), 0x2, 0); //放开CTRL8
            keybd_event(16, MapVirtualKey(16, 0), 0x2, 0);//放开shift键。
            keybd_event(116, MapVirtualKey(116, 0), 0x2, 0);//放开F5键。
        }
        public void Music_next()                        // ctrl+shif+A 下一曲
        {
            keybd_event(17, MapVirtualKey(17, 0), 0, 0); //按下CTRL           
            keybd_event(16, MapVirtualKey(16, 0), 0, 0);//键下shift键。
            keybd_event(65, MapVirtualKey(65, 0), 0, 0);//键下A键。
            keybd_event(17, MapVirtualKey(17, 0), 0x2, 0); //放开CTRL
            keybd_event(16, MapVirtualKey(16, 0), 0x2, 0);//放开shift键。
            keybd_event(65, MapVirtualKey(65, 0), 0x2, 0);//放开A键。
        }
        public void Music_last()                        // ctrl+shif+B 下一曲
        {
            keybd_event(17, MapVirtualKey(17, 0), 0, 0); //按下CTRL           
            keybd_event(16, MapVirtualKey(16, 0), 0, 0);//键下shift键。
            keybd_event(66, MapVirtualKey(66, 0), 0, 0);//键下B键。
            keybd_event(17, MapVirtualKey(17, 0), 0x2, 0); //放开CTRL
            keybd_event(16, MapVirtualKey(16, 0), 0x2, 0);//放开shift键。
            keybd_event(66, MapVirtualKey(66, 0), 0x2, 0);//放开B键。
        }
    }
}
