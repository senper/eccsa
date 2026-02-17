import { gql, useMutation, useQuery } from "urql";

const ASSOCIATIONS_QUERY = gql`
  query AssociationsQuery {
    associations {
      id
      nameEng
      nameLocal
      memberSince
      associationType
      associationLevel
      parentId
    }
  }
`;

const ASSOCIATION_DETAIL_QUERY = gql`
  query AssociationDetailQuery($id: Uuid!) {
    associationById(id: $id) {
      id
      nameEng
      nameLocal
      memberSince
      associationType
      associationLevel
      parentId
      boardMembers {
        id
        nameEng
        nameLocal
        startDate
        endDate
        status
      }
    }
  }
`;

export { ASSOCIATIONS_QUERY, ASSOCIATION_DETAIL_QUERY };
