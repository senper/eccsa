import React from "react";
import { useRouter } from "next/router";
import Link from "next/link";
import { logoff } from "../../api/oauth";
import { NavDropdown } from "react-bootstrap";
import { userMenu, userOtherMenus } from "../../pages/api/menu";

export function LoginMenu({ store, user, setUser, getLocal }) {
  const history = useRouter();
  function logOut() {
    logoff(store);
    setUser(null);
    history.push("/");
  }

  const DropdownItem = ({ item }) => {
    return (
      <Link href={item.path} passHref legacyBehavior>
        <NavDropdown.Item title={getLocal(item.title)}>{getLocal(item.text)}</NavDropdown.Item>
      </Link>
    );
  };
  return user ? (
    <>
      {
        <style jsx>
          {`
            div.loginMenu .nav-link,
            div.loginMenu .nav-link:hover,
            div.loginMenu .nav-link:focus {
              color: white;
            }
          `}
        </style>
      }
      <div
        className="btn-group loginMenu"
        role="group"
        aria-label="Basic example"
        style={{ float: "right", margin: "3px 25px 25px 25px" }}
      >
        <NavDropdown
          title={user ? user.fullName : getLocal("My page")}
          className="btn btn-lg btn-success"
          style={{ margin: "0px" }}
        >
          {userMenu.map((menu, index) =>
            !menu.businessAccountMenu || (user && user.businessAccount) ? (
              <DropdownItem item={menu} key={index} />
            ) : null
          )}
          <NavDropdown.Divider />
          {userOtherMenus.map((menu, index) =>
            !menu.businessAccountMenu || (user && user.businessAccount) ? (
              <DropdownItem item={menu} key={index} />
            ) : null
          )}
          <NavDropdown.Item onClick={logOut}>{getLocal('Logout')}</NavDropdown.Item>
        </NavDropdown>
      </div>
    </>
  ) : (
    <Link href="/login" legacyBehavior>
      <a
        className="btn btn-outline-success"
        style={{
          float: "right",
          margin: "3px 25px 25px 25px",
          backgroundColor: "#0d8523",
          color: "#fff",
        }}
      >
        {(getLocal("Login"))}/{(getLocal("Register"))}
      </a>
    </Link>
  );
}
