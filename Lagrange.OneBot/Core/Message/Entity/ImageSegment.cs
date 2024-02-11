using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class ImageSegment(string url)
{
    public ImageSegment() : this("") { }

    [JsonPropertyName("file")]  [CQProperty] public string File { get; set; } = url;
    
    [JsonPropertyName("url")] public string Url { get; set; }  = url;
}

[SegmentSubscriber(typeof(ImageEntity), "image")]
public partial class ImageSegment : ISegment
{
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is ImageSegment imageSegment and not { File: "" } && CommonResolver.Resolve(imageSegment.File) is { } image)
        {
            builder.Image(image);
        }
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not ImageEntity imageEntity) throw new ArgumentException("Invalid entity type.");

        return new ImageSegment(imageEntity.ImageUrl);
    }
}