import { CLEAR_SUBSCRIPTIONS, CLEAR_TOPICS, STORE_SUBSCRIPTIONS, STORE_TOPICS } from "./types";

export function storeTopics(topics) {
    return {
        type: STORE_TOPICS,
        payload: topics
    }
}

export function clearTopics() {
    return {
        type: CLEAR_TOPICS,
    }
}

export function storeSubscriptions(subscriptions) {
    return {
        type: STORE_SUBSCRIPTIONS,
        payload: subscriptions
    }
}

export function clearSubscriptions() {
    return {
        type: CLEAR_SUBSCRIPTIONS,
    }
}