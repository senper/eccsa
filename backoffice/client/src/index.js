/*!

=========================================================
* Material Dashboard PRO React - v1.10.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-pro-react
* Copyright 2021 Creative Tim (https://www.creative-tim.com)

* Coded by Creative Tim

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router } from "react-router-dom";

import App from "./App";

import { Provider, createClient, fetchExchange, dedupExchange } from "urql";

import { cacheExchange } from "@urql/exchange-graphcache";
import { USER_PROFILE_QUERY, SIGNUP_MUTATION } from "variables/user_gql";

const cache = cacheExchange({
  updates: {
    Mutation: {
      logout: (_, __, cache) => {
        console.log(cache);
      },
    },
  },
});

const client = createClient({
  url: "https://localhost:44397/graphql/",
  fetchOptions: () => {
    const token = localStorage.getItem("access_token");
    return {
      headers: { authorization: token ? `Bearer ${token}` : "" },
    };
  },
  exchanges: [dedupExchange, /* cache,  */ fetchExchange],
});

import "assets/scss/material-dashboard-pro-react.scss?v=1.10.0";

ReactDOM.render(
  <Router>
    <Provider value={client}>
      <App />
    </Provider>
  </Router>,
  document.getElementById("root")
);
