using Amazon.CDK;
using Amazon.CDK.AWS.Amplify;
using Amplify = Amazon.CDK.AWS.Amplify;

namespace S2VX.Deploy {
    public class S2VXStack : Stack {
        internal S2VXStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props) {
            // The code that defines your stack goes here
            var amplifyApp = new Amplify.App(this, "S2VX", new Amplify.AppProps {
                SourceCodeProvider = new GitHubSourceCodeProvider(new GitHubSourceCodeProviderProps {
                    Owner = "maxrchung",
                    Repository = "S2VX",
                    OauthToken = SecretValue.PlainText(
                        System.Environment.GetEnvironmentVariable("PERSONAL_ACCESS_TOKEN")
                    )
                }),
            });
            amplifyApp.AddBranch("amplify-deploy");
        }
    }
}
