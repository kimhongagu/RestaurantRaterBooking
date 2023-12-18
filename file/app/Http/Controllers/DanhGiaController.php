<?php

namespace App\Http\Controllers;

use App\Models\DanhGia;
use Illuminate\Http\Request;

class DanhGiaController extends Controller
{
    /**
     * Display a listing of the resource.
     */
    public function getDanhSach()
    {
        $danhgia = DanhGia::all();
        return view('danhgia.danhsach', compact('danhgia'));
    }
    public function getThem()
    {
        return view('danhgia.them');
    }
    public function postThem(Request $request)
    {
        $orm = new DanhGia();
        $orm->diemdanhgia = $request->diemdanhgia;
        $orm->nhanxet = $request->nhanxet; 
        $orm->save();
        return redirect()->route('danhgia');
    }
    public function getSua($id)
    {
        $danhmuc = DanhMuc::find($id);
        return view('danhmuc.sua', compact('danhmuc'));
    }
    public function postSua(Request $request, $id)
    {
        $orm = DanhMuc::find($id);
        $orm->tendanhmuc = $request->tendanhmuc;
        $orm->tendanhmuc_slug = Str::slug($request->tendanhmuc, '-');
        $orm->save();
        return redirect()->route('danhmuc');
    }
    public function getXoa($id)
    {
        $orm = DanhMuc::find($id);
        $orm->delete();
        return redirect()->route('danhmuc');
    }
}
