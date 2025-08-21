using mailGenerator;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace interactObjects
{
    /// <summary>
    /// Handles drag-and-drop functionality for game objects with shadow effects.
    /// </summary>
    public class DragDropMovement : MonoBehaviour // PascalCase for class name
    {
        // --- Serialized Fields ---
        [SerializeField] private float _shadowOffset = 0.3f; // Underscore prefix for private fields
        [SerializeField] private MainDataset _mainDataset;

        // --- Private Fields ---
        private bool _isDragging = false;
        private Vector2 _mousePosition;
        private Vector2 _dragOffset;
        private GameObject _objectShadow;
        private AudioSourcePool _audioSourcePool;
        private ScoreTracker _scoreTracker;
        private PauseScreen _pauseScreen;

        private void Awake()
        {
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
            _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker")?.GetComponent<ScoreTracker>();
            _pauseScreen = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<PauseScreen>();
            

            float size = _mainDataset.universalSize;
            transform.localScale = new Vector3(size, size, size);
        }

        /// <summary>
        /// Called when mouse button is pressed down on the object.
        /// Initializes dragging and creates a shadow clone.
        /// </summary>
        private void OnMouseDown()
        {
            //Can't dragdrop when game paused or game done fading in yet
            if (!_scoreTracker.IsStartDay || _pauseScreen.IsGamePaused) return; 

            Destroy(GetComponent<SpawnSlideMovement>());
            transform.SetAsLastSibling();
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dragOffset = _mousePosition - (Vector2)transform.position;

            _isDragging = true;
            _audioSourcePool.SFX_PaperUnFold.Play();

            // Create shadow clone
            Vector3 shadowPosition = new Vector3(
                transform.position.x - _shadowOffset,
                transform.position.y - _shadowOffset,
                transform.position.z
            );
            _objectShadow = Instantiate(
                    gameObject,
                    shadowPosition,
                    Quaternion.identity, transform.parent);

            // Ensure shadow appears right behind the dragged object
            SetUpShadowComponents(_objectShadow);

        }

        private void updateSortingLayerChildren()
        {
            int order = 0;
            foreach (Transform child in transform.parent.transform)
            {
                child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x,
                    child.gameObject.transform.position.y, order);
                order--;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, order - 1);

        }

        /// <summary>
        /// Configures the shadow object by removing unnecessary components.
        /// </summary>
        /// <param name="shadow">The shadow GameObject to configure</param>
        private void SetUpShadowComponents(GameObject shadow) // PascalCase for method name
        {
            shadow.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0f);

            // Remove all child objects
            foreach (Transform child in shadow.transform)
            {
                Destroy(child.gameObject);
            }

            // Configure shadow appearance
            shadow.name = "Object Shadow";
            shadow.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.8f);

            // Remove unnecessary components
            Destroy(shadow.GetComponent<Animator>());
            Destroy(shadow.GetComponent<BoxCollider2D>());

            // Remove specific script components if they exist
            Destroy(shadow.GetComponent<LetterReader>());
            Destroy(shadow.GetComponent<MailOpener>());
            Destroy(shadow.GetComponent<DragDropMovement>());
            Destroy(shadow.GetComponent<TicketSFX>());
            if (gameObject.CompareTag("Ticket")) Destroy(GetComponent<Animator>());

            // Set shadow as last child (will change later in Update)
            shadow.transform.SetAsLastSibling();


            StartCoroutine(updateTransparencyOfShadow(shadow));
        }

        /// <summary>
        /// Called when mouse button is released.
        /// Stops dragging and destroys the shadow.
        /// </summary>
        private void OnMouseUp()
        {
            if (_objectShadow != null)
            {
                Destroy(_objectShadow);
            }
            _isDragging = false;
        }

        /// <summary>
        /// Updates the object's position to follow the mouse while dragging.
        /// Also updates the shadow position.
        /// </summary>
        private void UpdateObjectPosition()
        {
            if (!_isDragging) return;

            transform.position = _mousePosition - _dragOffset;

            if (_objectShadow != null)
            {
                //Shadow must always be offsetted by a certain degree
                _objectShadow.transform.position = new Vector3(
                    transform.position.x - _shadowOffset,
                    transform.position.y - _shadowOffset,
                    transform.position.z
                );
                _objectShadow.transform.rotation = transform.rotation;
            }

            updateSortingLayerChildren();
        }

        private System.Collections.IEnumerator updateTransparencyOfShadow(GameObject _shadowObj)
        {
            yield return new WaitForSeconds(0.001f);
            _shadowObj.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 1f);
        }


        private void Update()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Maintain shadow ordering
            // Keep shadow immediately behind the dragged object
            if (_objectShadow != null) _objectShadow.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            UpdateObjectPosition();
        }

    }
}