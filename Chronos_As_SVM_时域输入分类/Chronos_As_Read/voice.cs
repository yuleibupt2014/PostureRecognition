using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis; //用于生成响应的事件
using System.Speech;
using System.Speech.Recognition;



namespace Chronos_As_Read
{
    class voice
    {
        SpeechSynthesizer voice1 = new SpeechSynthesizer();
        public void Analyse(string strSpeak)
        {
            int iCbeg = 0;
            int iEbeg = 0;
            bool IsChina = true;
            for (int i = 0; i < strSpeak.Length; i++)
            {
                char chr = strSpeak[i];
                if (IsChina)
                {
                    if (Convert.ToInt32(chr) <= 122 && Convert.ToInt32(chr) >= 65)
                    {
                        int iLen = i - iCbeg;
                        string strValue =
             strSpeak.Substring(iCbeg, iLen);
                        SpeakChina(strValue);
                        iEbeg = i;
                        IsChina = false;
                    }
                }
                else
                {
                    if (Convert.ToInt32(chr) > 122 || Convert.ToInt32(chr) < 65)
                    {
                        int iLen = i - iEbeg;
                        string strValue = strSpeak.Substring(iEbeg, iLen);
                        this.SpeakEnglishi(strValue);
                        iCbeg = i;
                        IsChina = true;
                    }
                }
            }
            if (IsChina)
            {
                int iLen = strSpeak.Length - iCbeg;
                string strValue = strSpeak.Substring(iCbeg, iLen);
                SpeakChina(strValue);
            }
            else
            {
                int iLen = strSpeak.Length - iEbeg;
                string strValue = strSpeak.Substring(iEbeg, iLen);
                SpeakEnglishi(strValue);
            }


        }

        public void SpeakChina(string speak)
        {
            voice1.SelectVoice("Microsoft Lili");                                //SpeakChina
            voice1.Speak(speak);
        }
        //英文
        public void SpeakEnglishi(string speak)
        {
            voice1.SelectVoice("Microsoft Anna");                               //SpeakEnglish
            voice1.Speak(speak);
        }

        public voice(int Volume,int Rate)
        {
            SpeechSynthesizer voice1 = new SpeechSynthesizer();
            voice1.Volume = Volume;
            voice1.Volume = Rate;
        }

    }
}
