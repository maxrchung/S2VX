using Amazon.CDK;
using Amazon.CDK.AWS.Amplify;
using Amazon.CDK.AWS.Route53;
using Amplify = Amazon.CDK.AWS.Amplify;

namespace S2VX.Deploy {
    public class S2VXStack : Stack {
        internal S2VXStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props) {
            new PublicHostedZone(this, "S2VX.HostedZone", new PublicHostedZoneProps {
                ZoneName = "s2vx.com"
            });

            var app = new Amplify.App(this, "S2VX.Amplify", new Amplify.AppProps {
                SourceCodeProvider = new GitHubSourceCodeProvider(new GitHubSourceCodeProviderProps {
                    Owner = "maxrchung",
                    Repository = "S2VX",
                    OauthToken = SecretValue.PlainText(
                        System.Environment.GetEnvironmentVariable("PERSONAL_ACCESS_TOKEN")
                    )
                }),
            });

            var branch = app.AddBranch("amplify-deploy");
            var domain = app.AddDomain("s2vx.com");
            domain.MapRoot(branch);
        }
    }
}
