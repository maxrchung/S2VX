using Amazon.CDK;
using Amazon.CDK.AWS.Amplify;
using Amplify = Amazon.CDK.AWS.Amplify;

namespace S2VX.Deploy {
    public class S2VXStack : Stack {
        internal S2VXStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props) {
            var app = new Amplify.App(this, "S2VX.Amplify", new Amplify.AppProps {
                // Configurations for Amplify to find and access our repo
                SourceCodeProvider = new GitHubSourceCodeProvider(new GitHubSourceCodeProviderProps {
                    Owner = "maxrchung",
                    Repository = "S2VX",
                    OauthToken = SecretValue.PlainText(
                        // Personal access token generated for my GitHub account
                        System.Environment.GetEnvironmentVariable("PERSONAL_ACCESS_TOKEN")
                    )
                }),
            });

            // Links our s2vx.com domain to the repository branch
            var branch = app.AddBranch("amplify-deploy");
            var domain = app.AddDomain("s2vx.com");
            domain.MapRoot(branch);
        }
    }
}
