
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Xml.Schema;

namespace HelloTriangle
{
    public class Game : GameWindow
    {
        private int CtrlBufer;
        private int CtrlShaderPrograma;
        private int trlArrayVertice;
        public Game(int width, int height, string title) 
            : base(GameWindowSettings.Default, new NativeWindowSettings() 
            { Size = (width, height), Title = title }) 
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
          //  KeyboardState input = KeyboardState;

         //   if (input.IsKeyDown(Keys.Escape))
         //    {
         //       Close();
         //   }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.CtrlShaderPrograma);
            GL.BindVertexArray(trlArrayVertice);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.CtrlBufer);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3*sizeof(float),0 );
            GL.EnableVertexAttribArray(0);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0,0,e.Width,e.Height);
        }
        protected override void OnLoad()
        {   GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1f)); 
            float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
            };
            CtrlBufer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, CtrlBufer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.trlArrayVertice = GL.GenVertexArray();
            GL.BindVertexArray(this.trlArrayVertice);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.CtrlBufer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            string vertexShaderCode = @"
              #version 330 core
                layout(location = 0) in vec3 aPosition;
                void main()
                {
                    gl_Position = vec4(aPosition, 1f);
                }";
            string pixelShaderCode =
                @"
                #version 330 core
                out vec4 pixelColor
                void main()
                {
                    pixelColor = vec4(0.8f, 0.8f, 0.1f, 1f)
                }
                ";
            int vertexShaderHandle=GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource (pixelShaderHandle, pixelShaderCode);   
            GL.CompileShader(pixelShaderHandle);

            this.CtrlShaderPrograma = GL.CreateProgram();
            GL.AttachShader(this.CtrlShaderPrograma, vertexShaderHandle);
            GL.AttachShader(this.CtrlShaderPrograma, pixelShaderHandle);

            GL.LinkProgram(this.CtrlShaderPrograma);   

            GL.DetachShader(this.CtrlShaderPrograma, vertexShaderHandle);
            GL.DetachShader (this.CtrlShaderPrograma, pixelShaderHandle); 

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);


            base.OnLoad();
        }
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.CtrlBufer);

            GL.UseProgram(0);
            GL.DeleteProgram(this.CtrlShaderPrograma);

            base.OnUnload();
        }
    }
}

