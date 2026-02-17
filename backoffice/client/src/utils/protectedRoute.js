/* eslint-disable react/prop-types */
import React from "react";
import { Route, Redirect } from "react-router-dom";
import { useSelector } from "react-redux";
import { loadUserFromStorage } from "services/oauth";
import store from "store";
import { USER_PROFILE_QUERY } from "variables/user_gql";
import { useQuery } from "urql";

function ProtectedRoute({ children, component: Component, ...rest }) {
  //var user = useSelector((state) => state.auth.user);
  var token = localStorage.getItem("access_token");
  const [userToken, setToken] = React.useState(token);
  const [result, executeQuery] = useQuery({
    query: USER_PROFILE_QUERY,
  });

  React.useEffect(() => {
    if (result.data) {
      const user = result.data.userProfile;
      if (token && !user) {
        setToken(undefined);
        // loadUserFromStorage(store).then((result) => {
        //   if (result == "Error") {
        //     logoff();
        //     token = undefined;
        //   }
        // });
      }
    }
  }, [result.data]);

  return userToken ? (
    <Route {...rest} component={Component} />
  ) : (
    <Redirect to={"/auth/login-page"} />
  );
}

export default ProtectedRoute;
