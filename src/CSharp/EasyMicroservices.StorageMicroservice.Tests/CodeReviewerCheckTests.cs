using CodeReviewer.Engine;
using EasyMicroservices.StorageMicroservice.Contracts;
using EasyMicroservices.StorageMicroservice.Controllers;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using EasyMicroservices.Tests;

namespace EasyMicroservices.StorageMicroservice.Tests
{
    public class CodeReviewerCheckTests : CodeReviewerTests
    {
        static CodeReviewerCheckTests()
        {
            //types to check (this will check all of types in assembly so no need to add all of types of assembly)
            AssemblyManager.AddAssemblyToReview(
                typeof(FileEntity),
                typeof(FileContract),
                typeof(FileController));
        }
    }
}