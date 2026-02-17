import { configureStore, ThunkAction, Action } from "@reduxjs/toolkit";
import rootReducer from './reducers';
import { createWrapper } from "next-redux-wrapper";

const initialState = {}

const makeStore = () =>
  configureStore({
    reducer: rootReducer,
    devTools: true,
    preloadedState: initialState
  });
  
  export const wrapper = createWrapper(makeStore);