import { gql } from "urql";

const TOPICS_QUERY = gql`
  query TopicsQuery {
    topics {
      id
      code
      nameEng
      nameLocal
      decription
      status
      category {
        id
        code
        nameEng
        nameLocal
      }
    }
  }
`;

const TOPIC_DETAIL_QUERY = gql`
  query TopicDetailQuery($id: Uuid!) {
    topicById(id: $id) {
      id
      code
      nameEng
      nameLocal
      decription
      decriptionLocal
      status
      categoryId
      category {
        id
        code
        nameEng
        nameLocal
      }
      resources {
        id
        nameEng
        nameLocal
        resourceType
        resourceContent
      }
      topicSettings {
        id
        settingVariableId
        settingValue
        settingVariable {
          code
          nameEng
          nameLocal
          dataType
          minValue
          maxValue
        }
      }
      trainings {
        id
        startDate
        endDate
        instructor
        status
      }
    }
  }
`;

const TOPIC_UPDATE_MUTATION = gql`
  mutation TopicUpdateMutation($topic: UpdateTopicInput!) {
    updateTopic(data: $topic) {
      payLoad {
        id
      }
    }
  }
`;

const TOPIC_CREATE_MUTATION = gql`
  mutation TopicCreateMutation($topic: CreateTopicInput!) {
    createTopic(data: $topic) {
      payLoad {
        id
      }
    }
  }
`;

export {
  TOPICS_QUERY,
  TOPIC_DETAIL_QUERY,
  TOPIC_UPDATE_MUTATION,
  TOPIC_CREATE_MUTATION,
};
