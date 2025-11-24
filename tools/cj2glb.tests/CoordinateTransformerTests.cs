namespace cj2glb.tests;
public class CoordinateTransformerTests
{
    [Test]
    public void TestTransform()
    {
        var crs = "urn:ogc:def:crs:EPSG::28992";
        var result = CoordinateTransformer.TransformToWGS84(91649.64, 435614.06, 87.552, crs);
        Assert.That(result[0] == 4.466620732706671);
        Assert.That(result[1] == 51.9054196717121);
    }
}
