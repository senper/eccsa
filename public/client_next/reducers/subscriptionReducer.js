import { CLEAR_SUBSCRIPTIONS, CLEAR_TOPICS, STORE_SUBSCRIPTIONS, STORE_TOPICS } from "../actions/types";

const initialState = {
  topics: null,
  subscriptions: null,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action) {
  switch (action.type) {
    case STORE_TOPICS:
      return {
        ...state,
        topics: action.payload,
      };
    case CLEAR_TOPICS:
      return {
        ...state,
        topics: null,
      };
    case STORE_SUBSCRIPTIONS:
      return {
        ...state,
        subscriptions: action.payload,
      };
    case CLEAR_SUBSCRIPTIONS:
      return {
        ...state,
        subscriptions: null,
      };
    default:
      return state;
  }
}
