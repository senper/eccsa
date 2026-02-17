using System;

namespace ECCSA.BackOffice.Server.Model.Common
{
	public interface IId
	{
		Guid Id { get; set; }

	}

	public interface ICode :IId
	{
		string Code { get; set; }
	}
	public interface IEntity : IId
	{

		Guid CreatedBy { get; set; }

		DateTime CreateStamp { get; set; }

		Guid UpdatedBy { get; set; }

		DateTime UpdateStamp { get; set; }

		bool IsValid();
	}

	public abstract class Entity : IEntity
	{
		public Guid Id { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreateStamp { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime UpdateStamp { get; set; }

		public virtual bool IsValid() { return false; }
		public override bool Equals(object obj)
		{
			var other = obj as Entity;
			if (ReferenceEquals(other, null))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			if (GetType() != other.GetType())
				return false;
			if (Id == Guid.Empty || other.Id == Guid.Empty)
				return false;
			return Id == other.Id;
		}
		public static bool operator ==(Entity a, Entity b)
		{
			if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
				return true;
			if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
				return false;
			return a.Equals(b);
		}

		public static bool operator !=(Entity a, Entity b)
		{
			return !(a == b);
		}
		public override int GetHashCode()
		{
			return (GetType().ToString() + Id).GetHashCode();
		}

		//protected  int GetHashCodeCore()
		//{
		//	//For each properties get the hashcode and ...
		//	unchecked
		//	{
		//		int hashCode = 13;

		//		hashCode = (hashCode * 7) ^ (Id.GetHashCode());
			
		//		//hashCode = (hashCode * 7) ^ (Id.GetHashCode());
		//		//hashCode = (hashCode * 7) ^ (Id.GetHashCode());
		//		//hashCode = (hashCode * 7) ^ (Id.GetHashCode());
		//		//hashCode = (hashCode * 7) ^ (Id.GetHashCode());
		//		//hashCode = (hashCode * 7) ^ (Id.GetHashCode());
		//		return hashCode;
		//	}
		//}
	}
}