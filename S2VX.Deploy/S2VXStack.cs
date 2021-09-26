using Amazon.CDK;
using Amazon.CDK.AWS.Amplify;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.Route53;
using System.Collections.Generic;
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
                // https://swimburger.net/blog/dotnet/how-to-deploy-blazor-webassembly-to-aws-amplify
                BuildSpec = BuildSpec.FromObjectToYaml(new Dictionary<string, object> {
                    { "Version", 1 },
                    {
                        "Applications",
                        new {
                            Frontend = new {
                                Phases = new {
                                    PreBuild = new {
                                        Commands = new[] {
                                            "curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh",
                                            "chmod +x *.sh",
                                            "./dotnet-install.sh -c 5.0 -InstallDir ./dotnet5",
                                            "./dotnet5/dotnet --version"
                                        }
                                    },
                                    Build = new {
                                        Commands = new[] { "./dotnet5/dotnet publish -c Release -o release" }
                                    }
                                },
                                Artifacts = new {
                                    BaseDirectory = "/release/wwwroot",
                                    Files = "**/*"
                                }
                            },
                            AppRoot = "S2VX.Web"
                        }
                    }
                }),
            });

            var branch = app.AddBranch("amplify-deploy");
            var domain = app.AddDomain("s2vx.com");
            domain.MapRoot(branch);
        }
    }
}
