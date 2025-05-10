using System.Windows;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;

///////////////////////////////////////
// Hilary Varela
// Cristobal Martinez
///////////////////////////////////////
// el error que sale antes de compilar
// es algo de git con las ramas 
// nose
///////////////////////////////////////

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
        // variables para la configuración de OpenGL
        private OpenGL gl;

        // variables para la iluminación
        private float[] light1posDiffusa = { 0.5f, 0.5f, 0.5f, 0.5f };
        private float[] light2posEspecular = { 0.0f, 14.0f, -8.0f, 0.0f };
        private float[] lmodel_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };

        // rotacion de la tetera
        float rotacion = 0.0f;

        // generador de números aleatorios
        private Random rand = new Random();

        // lista para almacenar las posiciones de las luces activas e inactivas
        private List<float[]> activeLightPositions = new List<float[]>();
        private List<float[]> inactiveLightPositions = new List<float[]>();
        
        // estado de la ultima posición de la luz
        private bool lastLightPositionState = false;

        // array de diferentes angulos de luces 
        private (uint LightId, float[] Position, float[] OnColor, float[] OffColor)[] lights =
        {
        (OpenGL.GL_LIGHT0, new float[]{ 0.0f, 20.0f, 10.0f, 1.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT1, new float[]{ 0.0f, 14.0f, -8.0f, 0.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT2, new float[]{ -10.0f, 5.0f, 0.0f, 1.0f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f }),
        (OpenGL.GL_LIGHT3, new float[]{0.5f, 0.5f, 8f, 8f }, new float[]{ 0.5f, 0.5f, 0.5f, 0.5f }, new float[]{ 0.0f, 0.0f, 0.0f, 0.0f })
        };

        // Metodo que se ejecuta al iniciar la aplicacion
        private void iniciar(object sender, EventArgs args)
        {
            // Configuracion de OpenGL
            OpenGL gl = openGLControl.OpenGL; ;
            // Configuracion de la proyección
            gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            // configuracion de la vista
            gl.Enable(OpenGL.GL_DEPTH_TEST);
        }

        // Método que se ejecuta al abrir la aplicación
        private void openGLControl_OpenGLDraw(object sender, EventArgs args)
        {
            // Configuración de OpenGL
            OpenGL gl = openGLControl.OpenGL;

            // Configuración de la proyección
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Configuración de la vista
            gl.LoadIdentity();

            // Configuración de los tipos de luces
            setupLighting(gl);

            // Configuración del color de la tetera
            float[] color = { 1.0f, 0.713f, 0.756f, 1.0f };

            // Configuración del material de la tetera
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE, color);

            // llama a la función de dibujar la tetera
            DrawTeapot(gl);

            // le da un poco de rotación a la tetera
            rotacion += 5.0f;
        }

        // metodo donde activa y desativa las luces según el checkbox
        private void setupLighting(OpenGL gl)
        {
            // Configura posiciones y estados base
            ConfigureLights(gl); 

            // Luz Ambiental Global
            lightAmbientalGlobal(gl);

            // Luz Ambiental (GL_LIGHT0)
            if (chkAmbientLight.IsChecked == true)
                // se activa la luz ambiental
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, lmodel_ambient);
            else
                // se desactiva la luz ambiental
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new float[] { 0, 0, 0, 1 });

            // Componente Difusa (controlada por checkbox independiente)
            if (chkDiffuseLight.IsChecked == true)
                // se activa la luz difusa
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light1posDiffusa);
            else
                // se desactiva la luz difusa
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new float[] { 0, 0, 0, 1 });

            // Componente Especular (controlada por checkbox independiente)
            if (chkSpecularLight.IsChecked == true)
                // se activa la luz especular
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light2posEspecular);
            else
                // se desactiva la luz especular
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, new float[] { 0, 0, 0, 1 });


            // Habilitar iluminación si al menos un componente esta activo
            bool lightingEnabled = chkGlobalAmbient.IsChecked == true
                                || chkAmbientLight.IsChecked == true
                                || chkDiffuseLight.IsChecked == true
                                || chkSpecularLight.IsChecked == true;

            // Habilitar o deshabilitar la iluminación
            if (lightingEnabled)
                // se habilita la iluminación atravez de un checkbox
                gl.Enable(OpenGL.GL_LIGHTING);
            else
                // se desactiva la iluminación
                gl.Disable(OpenGL.GL_LIGHTING);
        }

        // Método para configurar la luz ambiental global
        private void lightAmbientalGlobal(OpenGL gl)
        {
            if (chkGlobalAmbient.IsChecked == true)
                // se activa la luz ambiental global
                gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);
            else
                // se desactiva la luz ambiental global
                gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new float[] { 0, 0, 0, 1 });
        }

        // Metodo para configurar las posiciones de las luces usando un random para
        // que detecte si el checkbox de la luz está activado o no
        // y cada vez que habilite y dashabilite el checkbox tome una posicion diferente
        // segun las definidas en el array de luces
        private void ConfigureLights(OpenGL gl)
        {
            // crea una variable para almacenar el estado del checkbox
            bool currentState = chkLightPosition.IsChecked == true;

            // Si el estado del checkbox cambió, generamos nuevas posiciones
            if (currentState != lastLightPositionState)
            {
                // Limpiar las listas de posiciones
                List<float[]> targetList;

                if (currentState)
                {
                    // Si el checkbox esta activado, usamos la lista de posiciones activas
                    targetList = activeLightPositions;
                }
                else
                {
                    // Si el checkbox esta desactivado, usamos la lista de posiciones inactivas
                    targetList = inactiveLightPositions;
                }

                // despues de cada cambio limpia el estado del targetList
                targetList.Clear();

                // recorrer el array de luces y asignar posiciones aleatorias usando el random
                for (int i = 0; i < lights.Length; i++)
                {

                    float[] randomPos = {
                        (float)(rand.NextDouble() * 20 - 10),
                        (float)(rand.NextDouble() * 20 - 10),
                        (float)(rand.NextDouble() * 20 - 10),
                        1.0f
                    };
                    // Agregar la posición aleatoria a la lista correspondiente
                    targetList.Add(randomPos);
                }
                // Actualizar el estado de la última posicion de la luz
                lastLightPositionState = currentState;
            }

            // Usar la lista correspondiente según el estado
            List<float[]> positionsToUse;

            if (currentState)
            {
                // Si el checkbox esta activado, usamos la lista de posiciones activas
                positionsToUse = activeLightPositions;
            }
            else
            {
                // Si el checkbox esta desactivado, usamos la lista de posiciones inactivas
                positionsToUse = inactiveLightPositions;
            }

            // recorre el array de luces y asigna las posiciones
            for (int i = 0; i < lights.Length; i++)
            {
                // Asigna la posicion de la luz
                var light = lights[i];
                float[] pos = positionsToUse[i];

                // Configura la luz en OpenGL
                gl.Light(light.LightId, OpenGL.GL_POSITION, pos);
                gl.Light(light.LightId, OpenGL.GL_DIFFUSE, light.OnColor);
                gl.Light(light.LightId, OpenGL.GL_SPECULAR, light.OnColor);
                gl.Enable(light.LightId);
            }
        }


        // Metodo para configurar los eventos de los checkboxes
        private void SetupEventHandlers()
        {
            // Configura el evento de inicio (todos activos)
            var checkBoxes = new[] { chkGlobalAmbient, chkLightPosition, chkAmbientLight,
                               chkDiffuseLight, chkSpecularLight };

            // Asigna el evento Checked y Unchecked a cada checkbox
            foreach (var checkbox in checkBoxes)
            {
                // usando doRender para que cada vez que se active o desactive un checkbox
                // se renderice la escena
                checkbox.Checked += (s, e) => openGLControl.DoRender();
                checkbox.Unchecked += (s, e) => openGLControl.DoRender();
            }
        }

        // Método para dibujar la tetera
        private void DrawTeapot(OpenGL gl)
        {
            // Limpiamos la matriz de transformación
            gl.PushMatrix();

            // Posicionamos y escalamos la tetera
            gl.Translate(0.0f, 0.0f, -10.0f);
            gl.Rotate(rotacion, 0.0f, 1.0f, 0.0f);
            gl.Scale(1.5f, 1.5f, 1.5f);

            // Definimos una tetera a través de un objeto de superficie
            var teapot = new Teapot();

            // Configuramos el color de la tetera
            teapot.Render(gl, RenderMode.Render);
            gl.PopMatrix();
        }


    }
}