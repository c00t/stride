// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections;
using Stride.BepuPhysics.Definitions.Colliders;
using Stride.Core.Serialization;

namespace Stride.BepuPhysics.Definitions;

[DataSerializer(typeof(ListOfCollidersSerializer))]
internal sealed class ListOfColliders : IList<ColliderBase>, IReadOnlyList<ColliderBase>
{
    internal required CompoundCollider Owner { get; set; }

    readonly List<ColliderBase> _innerList = new();

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count => _innerList.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public void Add(ColliderBase item)
    {
        item.Component = Owner;
        _innerList.Add(item);
        Owner.OnEditCallBack();
    }

    /// <inheritdoc/>
    public void Insert(int index, ColliderBase item)
    {
        item.Component = Owner;
        _innerList.Insert(index, item);
        Owner.OnEditCallBack();
    }

    /// <inheritdoc/>
    public bool Remove(ColliderBase item)
    {
        if (_innerList.Remove(item))
        {
            item.Component = null;
            Owner.OnEditCallBack();
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        _innerList[index].Component = null;
        _innerList.RemoveAt(index);
        Owner.OnEditCallBack();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        foreach (var item in this)
            item.Component = null;
        _innerList.Clear();
        Owner.OnEditCallBack();
    }

    /// <inheritdoc/>
    public bool Contains(ColliderBase item) => _innerList.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(ColliderBase[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public int IndexOf(ColliderBase item) => _innerList.IndexOf(item);

    /// <inheritdoc cref="IList{T}.this" />
    public ColliderBase this[int index]
    {
        get
        {
            return _innerList[index];
        }
        set
        {
            value.Component = Owner; // Doing this one first just in case it throws
            _innerList[index].Component = null;
            _innerList[index] = value;
            Owner.OnEditCallBack();
        }
    }

    public List<ColliderBase>.Enumerator GetEnumerator() => _innerList.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator<ColliderBase> IEnumerable<ColliderBase>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class ListOfCollidersSerializer : DataSerializer<ListOfColliders>
{
    DataSerializer<ColliderBase> _itemDataSerializer = null!;

    public override void Initialize(SerializerSelector serializerSelector)
    {
        _itemDataSerializer = MemberSerializer<ColliderBase>.Create(serializerSelector);
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref ListOfColliders obj, ArchiveMode mode, SerializationStream stream)
    {
        if (mode == ArchiveMode.Deserialize)
        {
            if (obj == null!)
                obj = Activator.CreateInstance<ListOfColliders>();
            else
                obj.Clear();
        }
    }

    /// <inheritdoc/>
    public override void Serialize(ref ListOfColliders obj, ArchiveMode mode, SerializationStream stream)
    {
        if (mode == ArchiveMode.Deserialize)
        {
            int count = stream.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                ColliderBase value = null!;
                _itemDataSerializer.Serialize(ref value, mode, stream);
                obj.Add(value);
            }
        }
        else if (mode == ArchiveMode.Serialize)
        {
            stream.Write(obj.Count);
            foreach (var item in obj)
            {
                _itemDataSerializer.Serialize(item, stream);
            }
        }
    }
}
