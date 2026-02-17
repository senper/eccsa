import { gql, useMutation, useQuery } from "urql";

const APPLICANTS_QUERY = gql`
  query ApplicantsAllQuery {
    applicants {
      id
      nameEng
      nameLocal
      applicationDate
      legalStatus
      taxIdNumber
      associationId
      applicationStatus
    }
  }
`;

const APPLICANT_DETAIL_QUERY = gql`
  query ApplicantDetailQuery($id: Uuid!) {
    applicantById(id: $id) {
      id
      nameEng
      nameLocal
      applicationDate
      legalStatus
      taxIdNumber
      associationId
      applicationStatus
    }
  }
`;

export { APPLICANTS_QUERY, APPLICANT_DETAIL_QUERY };
