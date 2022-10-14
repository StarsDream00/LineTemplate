using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject lineHead, lineBody, wall, mainCamera;
    public bool wallMaker = false;
    private bool alive, start, direction, end;
    private float cameraSpeed;
    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        alive = true;
        start = false;
        direction = true;
        end = false;
        offset = mainCamera.transform.position - lineHead.transform.position;
        cameraSpeed = 0.03f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (start && !wallMaker && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
            return;
        }
        if (Input.anyKeyDown)
        {
            if (!start)
            {
                start = true;
                lineHead.GetComponent<AudioSource>().Play();
            }
            else if (!end)
            {
                direction = !direction;
            }
        }
    }

    private void FixedUpdate()
    {
        if (start && alive)
        {
            lineHead.transform.position += new Vector3(direction ? 0.25f : 0, 0, !direction ? 0.25f : 0);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, offset + lineHead.transform.position, cameraSpeed);
            if (wallMaker)
            {
                Instantiate(wall, lineHead.transform.position + new Vector3(2.5f, 0, -2.5f), lineHead.transform.rotation);
                Instantiate(wall, lineHead.transform.position + new Vector3(-2.5f, 0, 2.5f), lineHead.transform.rotation);
            }
            else
            {
                Instantiate(lineBody, lineHead.transform.position, lineHead.transform.rotation);
            }
        }
    }

    private void OnCollisionEnter(Collision x)
    {
        if (x.gameObject.CompareTag("Wall"))
        {
            alive = false;
            lineHead.GetComponent<AudioSource>().Stop();
            mainCamera.GetComponent<AudioSource>().Play();
        }
        else if (x.gameObject.CompareTag("End"))
        {
            offset *= 4;
            cameraSpeed = 0.015625f;
            end = true;
        }
    }
}
