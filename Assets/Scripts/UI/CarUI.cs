using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace UI
{
    
    [Serializable]
    public class CarUI : UIBase
    {
        public Image tachometerNeedle;
        public Image barShiftGUI;
        public Text speedText;
        public Text gearText;
        
        private int gearst = 0;
        private float thisAngle = -150;

        public override void InitUI()
        {
            ActivateUI(true);
        }
        
        public override void UpdateUI() {}
        
        public void UpdateUI(VehicleControl carScript)
        {
            gearst = carScript.currentGear;
            speedText.text = ((int)carScript.speed).ToString();

            if (carScript.carSetting.automaticGear)
            {

                if (gearst > 0 && carScript.speed > 1)
                {
                    gearText.color = Color.green;
                    gearText.text = gearst.ToString();
                }
                else if (carScript.speed > 1)
                {
                    gearText.color = Color.red;
                    gearText.text = "R";
                }
                else
                {
                    gearText.color = Color.white;
                    gearText.text = "N";
                }

            }
            else
            {

                if (carScript.NeutralGear)
                {
                    gearText.color = Color.white;
                    gearText.text = "N";
                }
                else
                {
                    if (carScript.currentGear != 0)
                    {
                        gearText.color = Color.green;
                        gearText.text = gearst.ToString();
                    }
                    else
                    {

                        gearText.color = Color.red;
                        gearText.text = "R";
                    }
                }

            }
        
            thisAngle = (carScript.motorRPM / 20) - 175;
            thisAngle = Mathf.Clamp(thisAngle, -180, 90);

            tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
            barShiftGUI.rectTransform.localScale = new Vector3(carScript.powerShift / 100.0f, 1, 1);
        }
    }
}