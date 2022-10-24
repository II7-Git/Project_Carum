package com.a101.carum.api.controller;

import com.a101.carum.api.dto.*;
import com.a101.carum.service.JwtService;
import com.a101.carum.service.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.io.UnsupportedEncodingException;
import java.sql.SQLIntegrityConstraintViolationException;

@RestController
@RequestMapping("user")
@RequiredArgsConstructor
public class UserController {

    private final UserService userService;
    private final JwtService jwtService;

    @PostMapping()
    public ResponseEntity createUser(@RequestBody ReqPostUser reqPostUser){
        userService.createUser(reqPostUser);
        return ResponseEntity.ok().build();
    }

    @PostMapping("login")
    public ResponseEntity loginUser(@RequestBody ReqLoginUser reqLoginUser) throws UnsupportedEncodingException {
        return ResponseEntity.ok(userService.loginUser(reqLoginUser));
    }

    @PostMapping("logout")
    public ResponseEntity logoutUser(HttpServletRequest request) {
        Long id = jwtService.getUserId(request);
        userService.logoutUser(id);
        return ResponseEntity.ok().build();
    }

    @GetMapping("")
    public ResponseEntity readUser(HttpServletRequest request) {
        Long id = jwtService.getUserId(request);
        return ResponseEntity.ok(userService.readUser(id));
    }

    @GetMapping("userid")
    public ResponseEntity readUserId(ReqGetUserId reqGetUserId) throws SQLIntegrityConstraintViolationException {
        userService.readUserId(reqGetUserId);
        return ResponseEntity.ok().build();
    }

    @GetMapping("nickname")
    public ResponseEntity readNickName(ReqGetNickName reqGetNickName) throws SQLIntegrityConstraintViolationException {
        userService.readNickName(reqGetNickName);
        return ResponseEntity.ok().build();
    }

    @PatchMapping()
    public ResponseEntity updateUser(@RequestBody ReqPatchUser reqPatchUser, HttpServletRequest request) {
        Long id = jwtService.getUserId(request);
        userService.updateUser(reqPatchUser, id);
        return ResponseEntity.ok().build();
    }
}
