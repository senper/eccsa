import React, { useEffect } from "react";
import {
  BrowserRouter as Router,
  Route,
  Switch,
  Redirect,
} from "react-router-dom";
import LoginPage from "./views/UserProfile/LoginPage";
import { Provider } from "react-redux";
import store from "./store";
import AuthProvider from "./utils/authProvider";
import oauth, { loadUserFromStorage } from "./services/oauth";
import PrivateRoute from "./utils/protectedRoute";

import AuthLayout from "layouts/Auth.js";
import MembershipLayout from "layouts/MembershipLayout.js";
import AcademyLayout from "layouts/AcademyLayout.js";
import TradePromotionLayout from "layouts/TradePromotionLayout.js";
import ConfigurationLayout from "layouts/ConfigurationLayout.js";

function App() {
  /* useEffect(() => {
    // fetch current user
    loadUserFromStorage(store);
  }, []); */

  return (
    <Switch>
      <PrivateRoute path="/membership" component={MembershipLayout} />
      <PrivateRoute path="/academy" component={AcademyLayout} />
      <PrivateRoute path="/tradePromotion" component={TradePromotionLayout} />
      <PrivateRoute path="/configuration" component={ConfigurationLayout} />

      <Route path="/login-page" component={LoginPage} />
      <Route path="/auth" component={AuthLayout} />

      <Redirect from="/" to="/login-page" />
    </Switch>
  );
}

export default App;
