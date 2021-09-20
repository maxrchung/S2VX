using Amazon.CDK;
using Amazon.CDK.AWS.S3;

namespace S2VX.Deploy {
    public class AppStack : Stack {
        internal AppStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props) =>
            // The code that defines your stack goes here
            new Bucket(this, "MyFirstBucket", new BucketProps {
                Versioned = true
            });
    }
}
