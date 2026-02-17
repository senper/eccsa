import { gql, useMutation, useQuery } from "urql";

const SIGNUP_MUTATION = gql`
  mutation SignupMutation($email: String!, $password: String!) {
    login(model: { email: $email, password: $password }) {
      token
    }
  }
`;

const USER_PROFILE_QUERY = gql`
  query UserProfileQuery {
    userProfile {
      id
      email
      email_verified
      name
      roles
      assignedModules
      defaultModule
      associationId
    }
  }
`;

const LOGOUT_QUERY = gql`
  mutation LogOutMutation {
    logout
  }
`;

export { SIGNUP_MUTATION, USER_PROFILE_QUERY, LOGOUT_QUERY };
