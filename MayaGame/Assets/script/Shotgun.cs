using UnityEngine;
using System.Collections;

public class Shotgun : Wepon {
    public int shotNum = 6;
    public bool pompAction = true;
    public bool tubeMag = true;



    protected override void MagDetach()
    {
        base.MagDetach();
        magazineTr.localPosition = new Vector3(-0.04f, -0.036f, -0.017f);
        magazineTr.localEulerAngles = new Vector3(0, -85f, 0);
    }

    public override void Shot(float ADSHosei = 2f)
    {
        for(int shot = 0; shot < shotNum; shot++)
        {
            base.Shot(ADSHosei);
        }
        if (pompAction)
        {
            StartCoroutine( PompAction(0.5f));
        }
        
    }

    protected override void ReloadEnd()
    {
        MagReturn();
        if (tubeMag)
        {
            if(magazine < pa.magazine && 0 < pa.totalAmmo)
            {
                if (magazine < pa.magazine - 1)
                {
                    anim.SetBool("ReloadLoop", true);
                }
                else
                {
                    CompReload();
                }
                magazine++;
                SendUI();
            }
            else
            {
                CompReload();
            }
        }
    }

    void CompReload()
    {
        anim.SetBool("Cock", true);
        anim.SetBool("ReloadLoop", false);
        if (weponAnim != null)
        {
            weponAnim.SetTrigger("Pomp");
        }
        reload = false;
        FPSCon.reload = false;
    }

    IEnumerator PompAction(float delay = 0.2f)
    {
        yield return new WaitForSeconds(delay);
        anim.SetTrigger("ShotCock");
        if (weponAnim != null)
        {
            weponAnim.SetTrigger("Pomp");
        }
    }
}
