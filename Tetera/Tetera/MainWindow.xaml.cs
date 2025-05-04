using System.Windows;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;


namespace Tetera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        private OpenGL gl;


        // Posiciones de luz definidas 
        private float[] light1posDiffusa = {0.5f, 0.5f, 0.5f, 0.5f };
        private float[] light2posEspecular = { 0.0f, 14.0f, -8.0f, 0.0f };
        private float[] global_ambient = { 0f, 0.0f, 0f, -12f };
        private float[] lmodel_ambient = { 2f, 2f, 2f, 1.0f };
        private float[] light6pos = { 8f, 50f, 60f, 4.0f };

        float rotacion = 0.0f;

        private (uint LightId, float[] Position, float[] OnColor, float[] OffColor)[] lights =
        {
        (OpenGL.GL_LIGHT0, new float[]{ 0.0f, 20.0f, 10.0f, 1.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT1, new float[]{ 0.0f, 14.0f, -8.0f, 0.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT2, new float[]{ -10.0f, 5.0f, 0.0f, 1.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT3, new float[]{0.5f, 0.5f, 8f, 8f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f })
        };

        private void iniciar(object sender, EventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL; ; // O también puedes usar: openGLControl.OpenGL
            gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            gl.Enable(OpenGL.GL_DEPTH_TEST); // Recomendado para 3D
        }

        private void openGLControl_OpenGLDraw(object sender, EventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();

            setupLighting(gl);

            float[] color = { 1.0f, 0.713f, 0.756f, 1.0f };
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE, color);

            DrawTeapot(gl);

            rotacion += 5.0f;
        }

        private void setupLighting(OpenGL gl)
        {
            ConfigureLights(gl); // Configura posiciones y estados base

            // Luz Ambiental Global
            lightAmbientalGlobal(gl);

            // Luz Ambiental (GL_LIGHT0)
            if (chkAmbientLight.IsChecked == true)
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, lmodel_ambient);
            else
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new float[] { 0, 0, 0, 1 });

            // Componente Difusa (controlada por checkbox independiente)
            if (chkDiffuseLight.IsChecked == true)
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light1posDiffusa);
            else
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new float[] { 0, 0, 0, 1 });

            // Componente Especular (controlada por checkbox independiente)
            if (chkSpecularLight.IsChecked == true)
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light2posEspecular);
            else
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, new float[] { 0, 0, 0, 1 });


            // Habilitar iluminación si al menos un componente está activo
            bool lightingEnabled = chkGlobalAmbient.IsChecked == true
                                || chkAmbientLight.IsChecked == true
                                || chkDiffuseLight.IsChecked == true
                                || chkSpecularLight.IsChecked == true;

            if (lightingEnabled)
                gl.Enable(OpenGL.GL_LIGHTING);
            else
                gl.Disable(OpenGL.GL_LIGHTING);
        }

        private void lightAmbientalGlobal(OpenGL gl) 
        {
            if (chkGlobalAmbient.IsChecked == true)
                gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient); 
            else
                gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new float[] { 0, 0, 0, 1 });
        }

        private void ConfigureLights(OpenGL gl)
        {
            foreach (var light in lights)
            {
                if (chkLightPosition.IsChecked == true)
                {
                    // Configurar posición y color de la luz activa
                    gl.Light(light.LightId, OpenGL.GL_POSITION, light.Position);
                    gl.Light(light.LightId, OpenGL.GL_DIFFUSE, light.OnColor);
                    gl.Light(light.LightId, OpenGL.GL_SPECULAR, light.OnColor);
                    gl.Enable(light.LightId);
                }
                else
                {
                    // Apagar la luz
                    gl.Light(light.LightId, OpenGL.GL_DIFFUSE, light.OffColor);
                    gl.Light(light.LightId, OpenGL.GL_SPECULAR, light.OffColor);
                    gl.Disable(light.LightId);
                }
            }
        }


        private void SetupEventHandlers()
        {
            var checkBoxes = new[] { chkGlobalAmbient, chkLightPosition, chkAmbientLight,
                               chkDiffuseLight, chkSpecularLight };

            foreach (var checkbox in checkBoxes)
            {
                checkbox.Checked += (s, e) => openGLControl.DoRender();
                checkbox.Unchecked += (s, e) => openGLControl.DoRender();
            }
        }
        private void DrawTeapot(OpenGL gl)
        {
            gl.PushMatrix();

            // Posicionamos y escalamos la tetera
            gl.Translate(0.0f, 0.0f, -10.0f);
            gl.Rotate(rotacion, 0.0f, 1.0f, 0.0f);
            gl.Scale(1.5f, 1.5f, 1.5f);

            // Definimos una tetera a través de un objeto de superficie
            var teapot = new Teapot();

            teapot.Render(gl, RenderMode.Render);
            gl.PopMatrix();
        }


    }
}