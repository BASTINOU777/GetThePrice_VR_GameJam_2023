using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
//using static UnityEngine.Rendering.DebugUI.Table;
//using UnityEngine.ProBuilder;
//using UnityEngine.ProBuilder.Shapes;
//using static UnityEditor.PlayerSettings;

public class Ennemy : MonoBehaviour
{
    [SerializeField] GameObject ennemy;
    [SerializeField] GameObject player;
    [SerializeField] GameObject sourceLaser;
    [SerializeField] GameObject projectile;

    //[SerializeField] GameObject target;
    [SerializeField]
    float vdeplacement = 0.4f, PasDeplacementX = 0.1f, PasDeplacementY = 0.1f, PasDeplacementZ = 0.1f,
        rayonmini = 20f, rayonmaxi = 40f, tpsPauseMini = 0.2f, tpsPauseMaxi = 1.5f, vitesseBoule = 100f;

    float Dymin, Dymax, Dxzmin, Dxyzmax, XDepart, YDepart, ZDepart;
    // maxX = 10f, maxY = 5f, maxZ = 10f, minY = 1.5f, minZ = -10f, minX = -10f,  Dymax = 4f, Dzmin = -10f, Dzmax = 10f, 
    //Vector3 origineEnnemy, originePlayer;

    //float speed = 2f;
    //float XArrive, YArrive, ZArrive;
    Vector3 PtArrive, PtDepart, offset;

    //const float Dxmin = -4f, Dxmax = 4f, Dymin = -1.5f, Dymax = 1.5f, Dzmin = -4f, Dzmax = 4f;
    //const float PasDeplacementX = 0.01f, PasDeplacementY = 0.01f, PasDeplacementZ = 0.01f;

    //float DistanceEnnemyPlayerX, DistanceEnnemyPlayerY, DistanceEnnemyPlayerZ;
    float tpsAttente;
    bool bouge, coroutineLancee, autorisationDeplacemment, deplacementEnCours;


    // Start is called before the first frame update
    void Start()
    {

        //origineEnnemy = new Vector3(XDepart, YDepart, ZDepart);
        //originePlayer= player.transform.position;
        //DistanceEnnemyPlayerX = originePlayer.x - origineEnnemy.x;
        //DistanceEnnemyPlayerY = originePlayer.y - origineEnnemy.y;
        //DistanceEnnemyPlayerZ = originePlayer.z - origineEnnemy.z;
        XDepart = 0;
        YDepart = 5;
        ZDepart = (rayonmaxi + rayonmini) / 2;
        bouge = false;
        autorisationDeplacemment = false;
        deplacementEnCours = false;
        coroutineLancee = false;
        tpsAttente = Random.Range(tpsPauseMini, tpsPauseMaxi);
        offset = player.transform.position;
        PtDepart = new Vector3(XDepart + offset.x, YDepart + offset.y, ZDepart + offset.z);
        Dymin = offset.y;
        Dymax = rayonmaxi / 2;
        Dxzmin = -rayonmaxi;
        Dxyzmax = rayonmaxi;

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 tmp;

        if (bouge == false && coroutineLancee == false)
        {
            // on attends tpsAttente seconde
            StartCoroutine(attend());
            Debug.Log("Coroutine lancer");
            coroutineLancee = true;
            //posIntermediaire = PtDepart;
            deplacementEnCours = false;
        }

        if (bouge == true)
        {
            coroutineLancee = false;


            // On calcule la nouvelle position
            if (!deplacementEnCours)
            {
                //Debug.Log("Pas de déplacement en cours, On tir et on se prépare à bouger !");
                tpsAttente = Random.Range(tpsPauseMini, tpsPauseMaxi);
                tir();
                PtArrive = nouvelleDestination(PtDepart);
                deplacementEnCours = true;
            }
            else
            {
                Debug.Log("Déplacement en cours, PtDepart= " + PtDepart + " PtArrive= " + PtArrive);
                PtDepart = gestionDeplacement(PtDepart, PtArrive);
                //if(tmp != Vector3.zero)
                //{
                //    PtDepart = tmp;
                //}

                if (finDuVoyage(PtDepart, PtArrive))
                {
                    Debug.Log("Fin du voyage renvoi true.");
                    bouge = false;
                    deplacementEnCours = false;
                }
                else
                {
                    deplacementEnCours = true;
                }

            }
            // Gestion de la vitesse de déplacement

        }

        //transform.Translate(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Vertical"));
        ////transform.Translate(Vector3.right * 5f * Time.deltaTime * Input.GetAxis("Horizontal"));
        //transform.Rotate(0f, speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0f);

    }

    private IEnumerator attend()
    {
        yield return new WaitForSeconds(tpsAttente);

        bouge = true;
        coroutineLancee = false;
        //Debug.Log("fin Coroutine tpsAttente " + tpsAttente + " bouge = " + bouge);
    }
    public IEnumerator vitesseDeplacement()
    {
        //Debug.Log("-> ENTREE Coroutine vdeplacement " + vdeplacement + " autorisationDeplacemment " + autorisationDeplacemment);

        yield return new WaitForSeconds(vdeplacement);

        autorisationDeplacemment = true;

        //Debug.Log("-> SORTIE Coroutine autorisationDeplacemment " + autorisationDeplacemment);


    }
    private void tir()
    {
        GameObject boule;
        //Debug.Log("Tir !!!! ");

        boule = Instantiate<GameObject>(projectile, sourceLaser.transform.position, sourceLaser.transform.rotation); //sourceLaser.transform.rotation

        boule.GetComponent<Rigidbody>().velocity = sourceLaser.transform.TransformDirection(Vector3.forward * vitesseBoule);// ennemy.transform.TransformDirection(Vector3.forward * (force));

        Destroy(boule, 1);

        //Debug.Log("Position : " + ProjectilePosition.transform.position);
        //balle.GetComponent<Rigidbody>().AddForce(ProjectilePosition.transform.forward);// = force;

    }

    private Vector3 nouvelleDestination(Vector3 dep)
    {
        Vector3 dest;
        float Dx, Dy, Dz, x2y2, x2y2z2;
        int signeZO = (dep.z < 0) ? -1 : 1;

        Dx = Random.Range(Dxzmin, Dxyzmax);
        Dy = Random.Range(Dxzmin / 2, Dxyzmax / 2);
        Dz = Random.Range(Dxzmin, Dxyzmax);

        dest.x = dep.x + Dx;
        dest.y = dep.y + Dy;
        dest.z = dep.z + Dz;

        //if (dest.x < Dxzmin + offset.x) dest.x = Dxzmin + offset.x;
        //if (dest.x > Dxyzmax + offset.x) dest.x = Dxyzmax + offset.x;
        //if (dest.y < Dymin + offset.y) dest.y = Dymin + offset.y;
        //if (dest.y > Dxyzmax + offset.y) dest.y = Dxyzmax + offset.y;
        //if (dest.z < Dxzmin + offset.z) dest.z = Dxzmin + offset.z;
        //if (dest.z > Dxzmin + offset.z) dest.z = Dxzmin + offset.z;

        if (dest.x < Dxzmin + offset.x) dest.x = Dxzmin + offset.x;
        if (dest.x > Dxyzmax + offset.x) dest.x = Dxyzmax + offset.x;

        if (dest.y < Dymin + offset.y) dest.y = Dymin + offset.y;
        if (dest.y > Dxyzmax + offset.y) dest.y = Dymax + offset.y;

        if (dest.z < Dxzmin + offset.z) dest.z = Dxzmin + offset.z;
        if (dest.z > Dxyzmax + offset.z) dest.z = Dxyzmax + offset.z;

        // Il faut s'assurer que la nouvelle position soit compris dans une sphére
        //  dont le rayon est entre un minimum et un maximum correspondant à la distance
        //  player/ennemy souhaité
        //if (dest.z * dest.z < Mathf.Abs(rayonmini * rayonmini - x2y2))
        //{
        //    dest.z = signeZO * Mathf.Sqrt(Mathf.Abs(rayonmaxi * rayonmaxi - x2y2));
        //}
        x2y2 = dest.x * dest.x + dest.y * dest.y;
        x2y2z2 = x2y2 + dest.z * dest.z;
        if (x2y2z2 < rayonmini) // Il faut "reculer" !
        {
            dest.z = -signeZO * Mathf.Sqrt(Mathf.Abs(rayonmini * rayonmini - x2y2));

        }
        //else if (x2y2z2 > rayonmaxi) // Il faut "avancer"!
        //{

        //}

        Debug.Log("On bouge vers x= " + dest.x + " y= " + dest.y + " z= " + dest.z + " signe= " + signeZO);
        Debug.Log("On bouge vers Dx= " + Dx + " Dy= " + Dy + " Dz= " + Dz);
        Debug.Log("depuis la position x= " + dep.x + " y= " + dep.y + " z= " + dep.z);

        return dest;
    }

    private Vector3 gestionDeplacement(Vector3 dep, Vector3 dest)
    {
        Vector3 posIntermediaire = dep;

        //bool pasEncoreArriveX, pasEncoreArriveY, pasEncoreArriveZ;

        StartCoroutine(vitesseDeplacement());

        if (autorisationDeplacemment)
        {
            int signeDeplacementX = (dest.x > dep.x) ? 1 : -1;
            int signeDeplacementY = (dest.y > dep.y) ? 1 : -1;
            int signeDeplacementZ = (dest.z > dep.z) ? 1 : -1;

            if (deplacementPossible(dep.x, dest.x, PasDeplacementX))
            {
                posIntermediaire.x = dep.x + signeDeplacementX * PasDeplacementX;
                Debug.Log("------> Autorisation deplacement en X : signeDeplacementX" + signeDeplacementX +
                    " dep.x " + dep.x);
            }
            //else
            //{
            //    pasEncoreArriveX = false;
            //}
            if (deplacementPossible(dep.y, dest.y, PasDeplacementY))
            {
                posIntermediaire.y = dep.y + signeDeplacementY * PasDeplacementY;
                Debug.Log("------> Autorisation deplacement en Y : signeDeplacementY" + signeDeplacementY +
                    " dep.y " + dep.y);
            }
            //else
            //{
            //    pasEncoreArriveY = false;
            //}
            if (deplacementPossible(dep.z, dest.z, PasDeplacementZ))
            {
                posIntermediaire.z = dep.z + signeDeplacementZ * PasDeplacementZ;
                Debug.Log("------> Autorisation deplacement en Z : signeDeplacementZ" + signeDeplacementZ +
                    " dep.z " + dep.z);
            }
            //else
            //{
            //    pasEncoreArriveZ = false;
            //}

            ennemy.transform.position = posIntermediaire;
            ennemy.transform.LookAt(player.transform.position); // 
            autorisationDeplacemment = false;
        }

        return posIntermediaire;

    }

    private bool finDuVoyage(Vector3 dep, Vector3 dest)
    {
        bool arrivee = false;

        if ((!deplacementPossible(dep.x, dest.x, PasDeplacementX)) &&
             (!deplacementPossible(dep.y, dest.y, PasDeplacementY)) &&
             (!deplacementPossible(dep.z, dest.z, PasDeplacementZ)))
        {
            arrivee = true;
            dep.x = dest.x;
            dep.y = dest.y;
            dep.z = dest.z;
            Debug.Log("Fin du voyage !" + dep + " / " + dest);
        }

        return (arrivee);
    }

    private bool deplacementPossible(float xd, float xa, float pas)
    {
        bool ret = (Mathf.Abs(xd - xa) > 1.1 * pas);

        Debug.Log("Deplacemet possible DP : xd " + xd + " xa " + xa + " pas " + pas + " ret " + ret);

        return ret;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            //Destroy(collision.gameObject);
        }

    }

}