import { combineReducers } from 'redux';
import authReducer from './authReducer';
import subscriptionReducer from './subscriptionReducer';

export default combineReducers({
    auth: authReducer,
    topicSubscriptions: subscriptionReducer
})