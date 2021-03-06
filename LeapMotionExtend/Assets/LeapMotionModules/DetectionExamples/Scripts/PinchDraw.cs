﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Leap.Unity.DetectionExamples {

      public class PinchDraw : MonoBehaviour {

        [Tooltip("Each pinch detector can draw one line at a time.")]
        [SerializeField]
        private PinchDetector[] _pinchDetectors;

        [SerializeField]
        private Material _material;

        [SerializeField]
        private Color _drawColor = Color.white;

        [SerializeField]
        private float _smoothingDelay = 0.01f;

        [SerializeField]
        private float _drawRadius = 0.002f;

        [SerializeField]
        private int _drawResolution = 8;

        [SerializeField]
        private float _minSegmentLength = 0.005f;

        private DrawState[] _drawStates;


        public Color DrawColor {
          get {
            return _drawColor;
          }
          set {
            _drawColor = value;
          }
        }

        public float DrawRadius {
          get {
            return _drawRadius;
          }
          set {
            _drawRadius = value;
          }
        }

        void OnValidate() {
          _drawRadius = Mathf.Max(0, _drawRadius);
          _drawResolution = Mathf.Clamp(_drawResolution, 3, 24);
          _minSegmentLength = Mathf.Max(0, _minSegmentLength);
        }

        void Awake() {
          if (_pinchDetectors.Length == 0) {
            Debug.LogWarning("No pinch detectors were specified!  PinchDraw can not draw any lines without PinchDetectors.");
          }
        }

        void Start() {
            if (Loptice == false)
            {
                KretanjeKosare.igra = false;
            }
            else
            {
                KretanjeKosare.igra = true;
            }
            if (Kosara == false)
            {
                KretanjeKosare.kosaraa = false;
            }
            else
            {
                KretanjeKosare.kosaraa = true;
            }


          _drawStates = new DrawState[_pinchDetectors.Length];
          for (int i = 0; i < _pinchDetectors.Length; i++) {
            _drawStates[i] = new DrawState(this);
          }
        }
        // DODANO NAKNADNO
        float vrijeme = 0.0f;
        float R = 0.0f;
        float G = 0.0f;
        float B = 0.0f;
        float A = 0.0f;

        public GameObject LopticaCrvena;
        public GameObject LopticaPlava;

        public bool PromjenaBojeSvakihPetSekundi;
        public bool PromjenaBojePostepeno;
        public bool PromjenaBojeSliders;
        public bool PromjenaBojeDuga;
        public bool PromjenaBojeSvirtualnimSliderima;
        public bool Loptice;
        public bool Kosara;

        public GameObject ShowColor;
        public GameObject PanelSaSLiderima;

        public Slider RBojaSlider;
        public Slider GBojaSlider;
        public Slider BBojaSlider;
        public Slider SizeSlider;

        float TempR;
        float TempG;
        float TempB;
        float TempSize;

        //
        public static float virtualTempR = 0.0f;
        public static float virtualTempG = 0.0f;
        public static float virtualTempB = 0.0f;
        public static float virtualTempSize = 0.0f;

        public static Vector3 PozicijaPalmaLijevaRuka;
        public static Vector3 PozicijaPalmaDesnaRuka;


        void Update()
        {
            // DODANO NAKNADNO
            vrijeme += Time.deltaTime;
            if (vrijeme > 0.5 && PromjenaBojeDuga == true)
            {
                float R = Random.Range(1, 254);
                float G = Random.Range(1, 254);
                float B = Random.Range(1, 254);

                _drawColor.r = R / 255;
                _drawColor.g = G / 255;
                _drawColor.b = B / 255;

            }
            if (PromjenaBojeSvirtualnimSliderima == true)
            {
                PanelSaSLiderima.gameObject.SetActive(true);
                _drawColor.r = virtualTempR;
                _drawColor.g = virtualTempG;
                _drawColor.b = virtualTempB;

                _drawRadius = virtualTempSize;
            }
            else if (PromjenaBojeSvirtualnimSliderima == false)
            {
                PanelSaSLiderima.gameObject.SetActive(false);
            }
            // ukoliko je vrijeme u sekundama vece od 5 te public bool varijabla check
            if (vrijeme > 5 && PromjenaBojeSvakihPetSekundi == true)
            {
                PromjeniBojuSvaihPetSek();
                vrijeme = 0.0f;
            }
            if (vrijeme > 0.5 && PromjenaBojePostepeno == true)
            {
                PromjeniBojuSvaihPetSek2();
                vrijeme = 0.0f;
            }
            if (PromjenaBojeSliders == true)
            {

                PromjenaBojeSaSliderima();
            }
            else if (PromjenaBojeSliders == false)
            {
                ShowColor.SetActive(false);
                RBojaSlider.gameObject.SetActive(false);
                GBojaSlider.gameObject.SetActive(false);
                BBojaSlider.gameObject.SetActive(false);
                SizeSlider.gameObject.SetActive(false);
            }


            //


            for (int i = 0; i < _pinchDetectors.Length; i++)
            {
                #region Za Crtanje
                if (Loptice == false)
                {
                    var detector = _pinchDetectors[i];
                    var drawState = _drawStates[i];

                    if (detector.DidStartPinch)
                    {
                        drawState.BeginNewLine();
                    }

                    if (detector.DidEndPinch)
                    {
                        drawState.FinishLine();
                    }

                    if (detector.IsPinching)
                    {
                        drawState.UpdateLine(detector.Position);
                    }
                }
               
                #endregion

                #region Za Pucanje

                if (Loptice == true)
                {
                    var detector = _pinchDetectors[i];                   

                    if (detector.DidStartPinch)
                    {
                        if (PozicijaPalmaLijevaRuka.x != 0.0f)
                        {
                            Instantiate(LopticaCrvena, PozicijaPalmaLijevaRuka, Quaternion.identity);
                        }
                        if (PozicijaPalmaDesnaRuka.x != 0.0f)
                        {
                            Instantiate(LopticaPlava, PozicijaPalmaDesnaRuka, Quaternion.identity);
                        }
                        PozicijaPalmaDesnaRuka = new Vector3(0, 0, 0);
                        PozicijaPalmaLijevaRuka = new Vector3(0, 0, 0);

                    }
                }

                #endregion

                


            }// for petlja
        }// update metoda


        void SpawnBullet()
        {

        }

        // DODANO NAKNADNO
        void PromjenaBojeSaSliderima()
        {
            TempR = RBojaSlider.value;
            TempG = GBojaSlider.value;
            TempB = BBojaSlider.value;

            TempSize = SizeSlider.value;

            _drawColor.r = TempR / 255;
            _drawColor.g = TempG / 255;
            _drawColor.b = TempB / 255;

            _drawRadius = TempSize * 0.001f;

            ShowColor.GetComponent<Renderer>().material.color = new Color(TempR / 255, TempG / 255, TempB / 255); 
        }

        void PromjeniBojuSvaihPetSek()
        {
            float R = Random.Range(0, 255);
            float G = Random.Range(0, 255);
            float B = Random.Range(0, 255);
            float A = Random.Range(0, 255);

            _drawColor.g = G / 255;
            _drawColor.r = R / 255;
            _drawColor.b = B / 255;
            _drawColor.a = A / 255;

        }

        void PromjeniBojuSvaihPetSek2()
        {
                //Debug.Log("A = " + A);
                if (A == 255 || B == 255 || R == 255 || G==255)
                {
                    A = 0;
                    B = 0;
                    G = 0;
                    R = 0;
                }
                if(A != 255)
                {
                    A += 15;
                }
                if (B != 255)
                {
                    B += 15;
                }
                if (R != 255)
                {
                    R += 15;
                }
                if (G != 255)
                {
                  G += 15;
                }

            _drawColor.r = R / 255;
            _drawColor.g = G / 255;
            _drawColor.b = B / 255;
            _drawColor.a = A / 255;

            }
        //

        private class DrawState {
          private List<Vector3> _vertices = new List<Vector3>();
          private List<int> _tris = new List<int>();
          private List<Vector2> _uvs = new List<Vector2>();
          private List<Color> _colors = new List<Color>();

          private PinchDraw _parent;

          private int _rings = 0;

          private Vector3 _prevRing0 = Vector3.zero;
          private Vector3 _prevRing1 = Vector3.zero;

          private Vector3 _prevNormal0 = Vector3.zero;

          private Mesh _mesh;
          private SmoothedVector3 _smoothedPosition;

          public DrawState(PinchDraw parent) {
            _parent = parent;

            _smoothedPosition = new SmoothedVector3();
            _smoothedPosition.delay = parent._smoothingDelay;
            _smoothedPosition.reset = true;
          }

          public GameObject BeginNewLine() {
            _rings = 0;
            _vertices.Clear();
            _tris.Clear();
            _uvs.Clear();
            _colors.Clear();

            _smoothedPosition.reset = true;

            _mesh = new Mesh();
            _mesh.name = "Line Mesh";
            _mesh.MarkDynamic();

            GameObject lineObj = new GameObject("Line Object");
            Proba1.iscrtanelinije.Add(lineObj);
            lineObj.transform.position = Vector3.zero;
            lineObj.transform.rotation = Quaternion.identity;
            lineObj.transform.localScale = Vector3.one;
            lineObj.AddComponent<MeshFilter>().mesh = _mesh;
            lineObj.AddComponent<MeshRenderer>().sharedMaterial = _parent._material;

            return lineObj;
          }

          public void UpdateLine(Vector3 position) {
            _smoothedPosition.Update(position, Time.deltaTime);

            bool shouldAdd = false;

            shouldAdd |= _vertices.Count == 0;
            shouldAdd |= Vector3.Distance(_prevRing0, _smoothedPosition.value) >= _parent._minSegmentLength;

            if (shouldAdd) {
              addRing(_smoothedPosition.value);
              updateMesh();
            }
          }

          public void FinishLine() {
            ;
            _mesh.UploadMeshData(true);
          }

          private void updateMesh() {
            _mesh.SetVertices(_vertices);
            _mesh.SetColors(_colors);
            _mesh.SetUVs(0, _uvs);
            _mesh.SetIndices(_tris.ToArray(), MeshTopology.Triangles, 0);
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
          }

          private void addRing(Vector3 ringPosition) {
            _rings++;

            if (_rings == 1) {
              addVertexRing();
              addVertexRing();
              addTriSegment();
            }

            addVertexRing();
            addTriSegment();

            Vector3 ringNormal = Vector3.zero;
            if (_rings == 2) {
              Vector3 direction = ringPosition - _prevRing0;
              float angleToUp = Vector3.Angle(direction, Vector3.up);

              if (angleToUp < 10 || angleToUp > 170) {
                ringNormal = Vector3.Cross(direction, Vector3.right);
              } else {
                ringNormal = Vector3.Cross(direction, Vector3.up);
              }

              ringNormal = ringNormal.normalized;

              _prevNormal0 = ringNormal;
            } else if (_rings > 2) {
              Vector3 prevPerp = Vector3.Cross(_prevRing0 - _prevRing1, _prevNormal0);
              ringNormal = Vector3.Cross(prevPerp, ringPosition - _prevRing0).normalized;
            }

            if (_rings == 2) {
              updateRingVerts(0,
                              _prevRing0,
                              ringPosition - _prevRing1,
                              _prevNormal0,
                              0);
            }

            if (_rings >= 2) {
              updateRingVerts(_vertices.Count - _parent._drawResolution,
                              ringPosition,
                              ringPosition - _prevRing0,
                              ringNormal,
                              0);
              updateRingVerts(_vertices.Count - _parent._drawResolution * 2,
                              ringPosition,
                              ringPosition - _prevRing0,
                              ringNormal,
                              1);
              updateRingVerts(_vertices.Count - _parent._drawResolution * 3,
                              _prevRing0,
                              ringPosition - _prevRing1,
                              _prevNormal0,
                              1);
            }

            _prevRing1 = _prevRing0;
            _prevRing0 = ringPosition;

            _prevNormal0 = ringNormal;
          }

          private void addVertexRing() {
                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    _vertices.Add(Vector3.zero);  //Dummy vertex, is updated later
                    _uvs.Add(new Vector2(i / (_parent._drawResolution - 1.0f), 0));
                    _colors.Add(_parent._drawColor);
                }
            }

          //Connects the most recently added vertex ring to the one before it
          private void addTriSegment() {
            for (int i = 0; i < _parent._drawResolution; i++) {
              int i0 = _vertices.Count - 1 - i;
              int i1 = _vertices.Count - 1 - ((i + 1) % _parent._drawResolution);

              _tris.Add(i0);
              _tris.Add(i1 - _parent._drawResolution);
              _tris.Add(i0 - _parent._drawResolution);

              _tris.Add(i0);
              _tris.Add(i1);
              _tris.Add(i1 - _parent._drawResolution);
            }
          }

          private void updateRingVerts(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal, float radiusScale) {
            direction = direction.normalized;
            normal = normal.normalized;

            for (int i = 0; i < _parent._drawResolution; i++) {
              float angle = 360.0f * (i / (float)(_parent._drawResolution));
              Quaternion rotator = Quaternion.AngleAxis(angle, direction);
              Vector3 ringSpoke = rotator * normal * _parent._drawRadius * radiusScale;
              _vertices[offset + i] = ringPosition + ringSpoke;
            }
          }
        }
      }
    }
