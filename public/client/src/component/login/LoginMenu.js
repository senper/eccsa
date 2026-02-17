import React from "react";
import { useSelector } from "react-redux";
import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import store from "../../store";
import { logoff, loadUserFromStorage } from "../../services/oauth";
import {
  storeUserError,
  storeUser,
  userSignedOut,
} from "../../actions/authActions";
import { NavDropdown } from "react-bootstrap";

export function LoginMenu() {
  //const user = useSelector((state) => state.auth.user);
  const history = useHistory();
  function logOut() {
    logoff();
    setToken(undefined);
  }
  var token = localStorage.getItem("access_token");
  const [userToken, setToken] = React.useState(token);
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );
  React.useEffect(() => {
    if ((!user || !user.id) && token) {
      setToken(token);
      loadUserFromStorage(store).then((userInput) => {
        token = localStorage.getItem("access_token");
        setToken(token);
        setUser(userInput ? userInput.data : null);
      });
    } else if (!token) {
      setUser({});
    }
  }, [token]);
  return token ? (
    <>
      <style>
        {` div.loginMenu .nav-link, div.loginMenu .nav-link:hover, div.loginMenu .nav-link:focus { color: white } `}
      </style>
      <div
        class="btn-group loginMenu"
        role="group"
        aria-label="Basic example"
        style={{ float: "right", margin: "25px" }}
      >
        {/* <Link className="btn btn-outline-success" to="/myServices" style={{margin: "0px"}}>My Page</Link>
        <button style={{margin: "0px"}}
          onClick={logOut}
          class="btn btn-sm btn-outline-warning"
        >
          Logout
        </button> */}
        <NavDropdown
          title={user ? user.fullName : "My page"}
          href="/myServices"
          id="basic-nav-dropdown"
          className="btn btn-sm btn-success"
          style={{ margin: "0px" }}
        >
          <NavDropdown.Item
            title="Requests By You"
            as={Link}
            to={{
              pathname: "/myServices",
              state: {
                eventKey: "myServices",
                navTo: "request",
              },
            }}
          >
            Requests
          </NavDropdown.Item>
          {user && user.businessAccount ? (
            <>
              <NavDropdown.Item
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "survey",
                  },
                }}
              >
                Surveys
              </NavDropdown.Item>
              <NavDropdown.Item
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "legalProfile",
                  },
                }}
              >
                {" "}
                Legal Documents
              </NavDropdown.Item>
              <NavDropdown.Item
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "exportAgreement",
                  },
                }}
              >
                Export Agreements
              </NavDropdown.Item>
              <NavDropdown.Item
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "contacts",
                  },
                }}
              >
                Contacts
              </NavDropdown.Item>
              <NavDropdown.Item
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "membership",
                  },
                }}
              >
                Membership
              </NavDropdown.Item>
            </>
          ) : null}
          <NavDropdown.Divider />
          <NavDropdown.Item
            as={Link}
            to={{
              pathname: "/profileForm",
              state: {
                eventKey: "myServices",
              },
            }}
          >
            Update Profile
          </NavDropdown.Item>
          <NavDropdown.Item
            as={Link}
            to={{
              pathname: "/changePassword",
              state: {
                eventKey: "myServices",
              },
            }}
          >
            Change Password
          </NavDropdown.Item>
          <NavDropdown.Item onClick={logOut}>Logout</NavDropdown.Item>
        </NavDropdown>
      </div>
    </>
  ) : (
    <Link
      class="btn btn-outline-success"
      to="/login"
      style={{ marginRight: "5px" }}
    >
      Login/Register
    </Link>
  );
}
