import React from "react";
import { Route, Redirect } from "react-router-dom";
import { useSelector } from "react-redux";
import store from "../store";
import { loadUserFromStorage } from "../services/oauth";
import {
  storeUserError,
  storeUser,
  userSignedOut,
} from "../actions/authActions";

function ProtectedRoute({ children, component: Component, ...rest }) {
  //const user = useSelector(state => state.auth.user);
  var token = localStorage.getItem("access_token");
  const [userToken, setToken] = React.useState(token);
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );
  React.useEffect(() => {
    if (!user && token) {
      setToken(token);
      loadUserFromStorage(store).then((user) => {
        token = localStorage.getItem("access_token");
        setToken(token);
      });
    }
  }, [token]);
  return token ? (
    <Route {...rest} component={Component} />
  ) : (
    <Redirect 
    to={{
      pathname: "/login",
      state: { redirect: rest.path, ...rest.location.state }
    }} />
  );
}

export default ProtectedRoute;
