import { useState, useEffect } from "react";
import Link from "next/link";
import { Navbar, Nav, NavDropdown, Container } from "react-bootstrap";
import styles from "../../styles/Layout.module.css";
import { useRouter } from "next/router";
import MenuSearchForm from "./MenuSearchForm";
import { getServiceOwners } from "../../pages/api/servicetype";
import { getMainMenus, getPathFromTitle } from "../../pages/api/menu";

export default function MainMenu({ user }) {
  const [active, setActive] = useState("default");
  const location = useRouter();
  const { locale } = location;

  const [serviceOwners, setServiceOwners] = useState([]);
  useEffect(() => {
    getServiceOwners().then((data) => {
      setServiceOwners(data.data.$values);
    });
  }, []);
  /////
  const [menus, setMenus] = useState([]);
  useEffect(() => {
    getMainMenus().then((data) => {
      setMenus(data.data.$values.filter(m => m.showOnMainMenu));
    });
  }, []);
  /////

  const getTitle = (item) => {
    return locale == "en" || !item.titleLocal ? item.title : item.titleLocal;
  };

  const getPath = (item) => {
    if (!item.path) return item.path;
    return (
      item.path // + (item.path.includes("?") ? "&" : "?") + `menuId=${item.id}`
    );
  };
  const MenuItem = ({ item }) => {
    if (
      item.children &&
      item.children.$values &&
      item.children.$values.filter(m => m.showOnMainMenu).length > 0
    ) {
      return (
        <NavDropdown href={item.path} title={getTitle(item)}>
          {serviceOwners && item.title == "Services"
            ? serviceOwners.map((so, index) => (
                <DropdownItem
                  item={{
                    title: getTitle(so),
                    path: `/services/${getPathFromTitle(so.title)}`,
                  }}
                  key={index}
                />
              ))
            : null}
          {item.children.$values.filter(m => m.showOnMainMenu).map((submenu, index) => (
            <DropdownItem
              item={submenu}
              key={index + (serviceOwners ? serviceOwners.length : 0)}
            />
          ))}
        </NavDropdown>
      );
    }
    return (
      <Link href={getPath(item)} passHref legacyBehavior>
        <Nav.Link className={location.pathname == item.path ? "active" : ""}>
          {getTitle(item)}
        </Nav.Link>
      </Link>
    );
  };

  const DropdownItem = ({ item }) => {
    return (
      <Link href={getPath(item)} passHref legacyBehavior>
        <NavDropdown.Item>{getTitle(item)}</NavDropdown.Item>
      </Link>
    );
  };
  return (
    <Navbar collapseOnSelect className={styles.main} expand="lg">
      <Container>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav
            className="me-auto"
            activeKey={active}
            onSelect={(selectedKey) => setActive(selectedKey)}
          >
            {menus.map((menu, index) => (
              <MenuItem item={menu} key={index} />
            ))}
            {user ? (
              <MenuItem item={{ title: "My Services", path: "/myServices" }} />
            ) : null}
          </Nav>
          <div className="searchform">
            <MenuSearchForm />
          </div>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
