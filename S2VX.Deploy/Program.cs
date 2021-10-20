using Amazon.CDK;

namespace S2VX.Deploy {
    internal sealed class Program {
        public static void Main() {
            var app = new App();
            new S2VXStack(app, "S2VX");
            app.Synth();
        }
    }
}
