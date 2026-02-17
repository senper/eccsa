import { gql } from "urql";

const TOPIC_CATEGORY_QUERY = gql`
  query TopicCategoryQuery {
    categories {
      id
      code
      nameEng
      nameLocal
    }
  }
`;

export { TOPIC_CATEGORY_QUERY };
